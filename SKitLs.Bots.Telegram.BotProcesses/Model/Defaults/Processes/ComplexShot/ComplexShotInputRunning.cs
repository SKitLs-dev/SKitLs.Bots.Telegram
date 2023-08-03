using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.ComplexShot
{
    /// <summary>
    /// The running version of the <see cref="ComplexShotInputRunning{TResult}"/>. See it for info.
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument, which must not be nullable and have a parameterless constructor.</typeparam>
    public class ComplexShotInputRunning<TResult> : TextInputsRunningBase<ComplexShotInputProcess<TResult>, ComplexShotArgument<TResult>> where TResult : notnull, new()
    {
        /// <summary>
        /// Represents the process arguments associated with the running bot process.
        /// </summary>
        public override ComplexShotArgument<TResult> Arguments { get; protected set; }
        /// <summary>
        /// Represents the bot process definition that launched this running process.
        /// </summary>
        public override ComplexShotInputProcess<TResult> Launcher { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexShotInputRunning{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns and initiated the running bot process.</param>
        /// <param name="args">The process arguments associated with the running bot process.</param>
        /// <param name="launcher">The bot process definition that launched this running process.</param>
        public ComplexShotInputRunning(long userId, ComplexShotArgument<TResult> args, ComplexShotInputProcess<TResult> launcher) : base(userId)
        {
            Arguments = args;
            Launcher = launcher;
        }

        /// <summary>
        /// Handles the input update of type <see cref="SignedMessageTextUpdate"/> for the running bot process.
        /// </summary>
        /// <param name="update">The update containing the input for the bot process.</param>
        public override async Task HandleInput(SignedMessageTextUpdate update)
        {
            Arguments.CompleteStatus = update.Text.ToLower() == TerminationalKey.ToLower()
                ? ProcessCompleteStatus.Canceled
                : ProcessCompleteStatus.Pending;

            if (Arguments.CompleteStatus == ProcessCompleteStatus.Pending)
            {
                var serializer = update.Owner.ResolveService<IArgsSerializeService>();
                // Try to unpack and parse data
                // Data counts wrong
                var res = serializer.Deserialize<TResult>(update.Text, '\n');

                //(ConvertResult<TResult>?)serializer.GetType()
                //    .GetMethod(nameof(serializer.Deserialize))!
                //    .MakeGenericMethod(typeof(TResult))
                //    .Invoke(serializer, new object[] { update.Text, '\n' });
                //var res = Serializer.Deserialize<IBotDisplayable>(update.Text, '\n');

                if (res is null || res.ResultType != ConvertResultType.Ok)
                {
                    var mes = new MultiblockMessage();
                    mes.AddBlock("Следующие данные были введены неверно:");
                    mes.AddBlock(res?.ResultMessage ?? "Внутренняя ошибка преобразования");
                    mes.AddBlock("Отредактируйте, пожалуйста, данные, и ведите их заново");
                    await update.Owner.DeliveryService.ReplyToSender(mes, update);

                    Arguments.CompleteStatus = ProcessCompleteStatus.Failure;
                }
                else
                {
                    Arguments.BuildingInstance = res.Value;
                    Arguments.CompleteStatus = ProcessCompleteStatus.Success;

                    //var mes = new MultiblockMessage();
                    //mes.AddBlock("Проверьте введённые данные:");
                    //mes.AddBlock(value.FullDisplay());
                    //var menu = new PairedInlineMenu()
                    //{
                    //    ColumnsCount = 2
                    //};
                    //menu.Add("Да", ConfirmCallback.GetSerializedData(new ConfrimArg<SignedCallbackUpdate>(true, proc), Serializer));
                    //menu.Add("Нет", ConfirmCallback.GetSerializedData(new ConfrimArg<SignedCallbackUpdate>(false, proc), Serializer));
                    //mes.Menu = menu;
                    //await Owner.DeliveryService.ReplyToSender(mes, update);
                }
            }

            await TerminateWithAsync(Arguments, update);
        }
    }
}