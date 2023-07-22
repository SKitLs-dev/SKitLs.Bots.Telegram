using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.BotProcesses.Model.DefaultProcesses.PartialInput;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.DefaultProcesses.ShotInput
{
    public class ShotInputProcess<TResult> : InputProcess<TResult> where TResult : notnull, new()
    {
        public IBuildableMessage StartupMessage { get; private set; }

        public ShotInputProcess(string processDefId, string terminationalKey, IUserState controllerState, InputProcessComplete<TResult> whenOver, IBuildableMessage startupMessage) : base(processDefId, terminationalKey, controllerState, whenOver)
        {
            StartupMessage = startupMessage;
        }

        public override IBotRunningProcess GetRunning(long userId, InputProcessArgs<TResult> args) => new ShotInputRunning<TResult>(userId, args.Value, this);
    }
    public class ShotInputRunning<TResult> : InputRunning<TResult>, IBotRunningProcess where TResult : notnull, new()
    {
        public ShotInputProcess<TResult> Launcher { get; private set; }
        public override InputProcessComplete<TResult> WhenOver => Launcher.WhenOver;

        public IBuildableMessage StartupMessage => Launcher.StartupMessage;

        public ShotInputRunning(long userId, TResult instance, ShotInputProcess<TResult> launcher)
            : base(userId, instance, launcher)
        {
            Launcher = launcher;
        }

        public override async Task LaunchWith<TUpdate>(TUpdate update)
        {
            IBuildableMessage mes;
            if (StartupMessage is IDynamicMessage dynamic)
            {
                mes = dynamic.BuildWith(update);
            }
            else
                mes = StartupMessage;
            await update.Owner.DeliveryService.ReplyToSender(mes, update);
        }
        public override async Task HandleInput(SignedMessageTextUpdate update)
        {
            if (update is not SignedMessageTextUpdate messageUpdate)
                throw new Exception();
            if (messageUpdate.Text.ToLower() == TerminationalKey.ToLower())
            {
                var result = InputProcessResult<TResult>.Canceled();
                await Terminate(result, messageUpdate);
            }
            else
            {
                var serializer = update.Owner.ResolveService<IArgsSerilalizerService>();
                // Try to unpack and parse data
                // Data counts wrong
                var res = serializer.Deserialize<TResult>(update.Text, '\n');

                //(ConvertResult<TResult>?)serializer.GetType()
                //    .GetMethod(nameof(serializer.Deserialize))!
                //    .MakeGenericMethod(typeof(TResult))
                //    .Invoke(serializer, new object[] { update.Text, '\n' });
                //var res = Serializer.Deserialize<IBotDisplayable>(update.Text, '\n');

                InputProcessResult<TResult> result;
                if (res is null || res.ResultType != ConvertResultType.Ok)
                {
                    var mes = new MultiblockMessage();
                    mes.AddBlock("Следующие данные были введены неверно:");
                    mes.AddBlock(res?.Message ?? "Внутренняя ошибка преобразования");
                    mes.AddBlock("Отредактируйте, пожалуйста, данные, и ведите их заново");
                    await update.Owner.DeliveryService.ReplyToSender(mes, update);

                    result = InputProcessResult<TResult>.Denied();
                }
                else
                {
                    BuildingInstance = res.Value;
                    result = InputProcessResult<TResult>.Accepted(BuildingInstance);

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
                await Terminate(result, messageUpdate);
            }
        }
    }
}
