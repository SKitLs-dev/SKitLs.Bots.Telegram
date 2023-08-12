using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Confirm;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Stateful.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults
{
    /// <summary>
    /// Represents an abstract base class for running bot processes with text inputs.
    /// </summary>
    /// <typeparam name="TProcess">The type of the bot process definition, derived from <see cref="TextInputsProcessBase{TResult}"/>.</typeparam>
    /// <typeparam name="TResult">The type of the resulting instance, which must not be nullable.</typeparam>
    public abstract class TextInputsRunningBase<TProcess, TResult> : IBotRunningProcess where TProcess : TextInputsProcessBase<TResult> where TResult : notnull
    {
        /// <summary>
        /// Represents the unique identifier of the bot process definition.
        /// It serves as a distinguishing identifier for the particular bot process within the application.
        /// </summary>
        public virtual string ProcessDefId => Launcher.ProcessDefId + $"Running.{OwnerUserId}";
        /// <summary>
        /// Represents the unique identifier of the user who owns and initiated the running bot process.
        /// It allows to identify and associate the bot process with the specific user who triggered its execution.
        /// </summary>
        public virtual long OwnerUserId { get; protected set; }

        /// <summary>
        /// Represents the process arguments associated with the running bot process.
        /// </summary>
        public abstract TextInputsArguments<TResult> Arguments { get; protected set; }
        /// <summary>
        /// Represents the bot process definition that launched this running process.
        /// </summary>
        public abstract TProcess Launcher { get; protected set; }
        /// <summary>
        /// Represents the action that is invoked when the running bot process is completed without confirmation.
        /// </summary>
        public virtual ProcessCompletedByInput<TResult>? ForcedOver => Launcher.ForcedOver;
        /// <summary>
        /// Represents the action that is invoked when the running bot process is completed after confirmation.
        /// </summary>
        public virtual ConfirmationProcess<TResult>? Confirmation => Launcher.Confirmation;

        /// <summary>
        /// Represents the key used to stop and terminate the bot process.
        /// </summary>
        public virtual string TerminationalKey => Launcher.TerminationalKey;
        /// <summary>
        /// Represents the state associated with the bot process.
        /// Allows to maintain user-specific state information, enabling the resumption of interrupted processes
        /// or tracking progress.
        /// </summary>
        public virtual IUserState ProcessState => Launcher.ProcessState;
        /// <summary>
        /// Represents the startup message of the bot process.
        /// </summary>
        public virtual DynamicArg<TResult> StartupMessage => Launcher.StartupMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputsRunningBase{TProcess, TArg}"/> class with the specified <paramref name="ownerUserId"/>.
        /// </summary>
        /// <param name="ownerUserId">The unique identifier of the user who owns and initiated the running bot process.</param>
        public TextInputsRunningBase(long ownerUserId)
        {
            OwnerUserId = ownerUserId;
        }

        /// <summary>
        /// Launches and starts the bot process with the provided update.
        /// </summary>
        /// <typeparam name="TUpdate">The type of trigger update that implements the <see cref="ISignedUpdate"/> interface.</typeparam>
        /// <param name="update">The update to launch the bot process with.</param>
        public virtual async Task LaunchWith<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate
        {
            // TODO
            if (StartupMessage is null) throw new Exception();

            var mes = await StartupMessage.Invoke(Arguments, update).BuildContentAsync(update);
            if (update is SignedCallbackUpdate callback)
                mes = new EditWrapper(mes, callback.TriggerMessageId);

            await update.Owner.DeliveryService.AnswerSenderAsync(mes, update);
        }

        /// <summary>
        /// Handles and processes input from the user with the provided update during the execution of the bot process.
        /// </summary>
        /// <typeparam name="TUpdate">The type of trigger update that implements the <see cref="ISignedUpdate"/> interface.</typeparam>
        /// <param name="update">The update to launch the bot process with.</param>
        public async Task HandleInput<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate
        {
            if (update is not SignedMessageTextUpdate messageUpdate)
                throw new UpdateMissMatchException(this, typeof(SignedMessageTextUpdate), typeof(TUpdate));
            await HandleInput(messageUpdate);
        }
        /// <summary>
        /// Handles the input update of type <see cref="SignedMessageTextUpdate"/> for the running bot process.
        /// </summary>
        /// <param name="update">The update containing the input for the bot process.</param>
        public virtual Task HandleInput(SignedMessageTextUpdate update)
        {
            Arguments.CompleteStatus = update.Text.ToLower() == TerminationalKey.ToLower()
                ? ProcessCompleteStatus.Canceled
                : ProcessCompleteStatus.Pending;
            return Task.CompletedTask;
        }

        protected virtual async Task HandleConversionAsync(ConvertResult<TResult> result, SignedMessageTextUpdate update)
        {
            if (result.ResultType != ConvertResultType.Ok)
            {
                Arguments.CompleteStatus = ProcessCompleteStatus.Failure;

                // TODO : Localize
                var mes = new MultiblockMessage();
                mes.AddBlock("Следующие данные были введены неверно:");
                mes.AddBlock(result?.ResultMessage ?? "Внутренняя ошибка преобразования");
                mes.AddBlock("Отредактируйте, пожалуйста, данные, и ведите их заново");

                await update.Owner.DeliveryService.AnswerSenderAsync(await mes.BuildContentAsync(update), update);
            }
            else
            {
                Arguments.CompleteStatus = ProcessCompleteStatus.Success;
                Arguments.BuildingInstance = result.Value;
            }
        }

        /// <summary>
        /// Terminates the running bot process with the specified result and update.
        /// </summary>
        /// <param name="update">The update associated with the bot process termination.</param>
        public virtual async Task TerminateAsync<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate
        {
            if (update.Sender is not IStatefulUser stateful)
                throw new NotStatefulException(this);

            var _pm = update.Owner.ResolveService<IProcessManager>();
            _pm.Terminate(stateful);

            if (ForcedOver is not null && update is SignedMessageTextUpdate text)
                await ForcedOver.Invoke(Arguments, text);

            if (Confirmation is not null)
            {
                await _pm.Run(Confirmation, Arguments, update).LaunchWith(update);
            }
        }
    }
}