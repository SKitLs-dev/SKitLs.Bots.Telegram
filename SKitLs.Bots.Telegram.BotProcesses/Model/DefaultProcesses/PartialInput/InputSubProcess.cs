using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Protoype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using System.Reflection;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.DefaultProcesses.PartialInput
{
    public class InputSubProcess<T> : ISubProcess<PartialInputRunning<T>>
    {
        public int SubProcId { get; internal set; }
        public bool IsTerminational => false;

        public PropertyInfo HandlingProperty { get; set; }
        public Func<ISignedUpdate, IBuildableMessage> OnLaunch { get; set; }
        public Func<SignedMessageTextUpdate, IBuildableMessage?>? InputPreview { get; set; }
        public Func<SignedMessageTextUpdate, object> ParseInput { get; set; }

        public InputSubProcess(PropertyInfo property, IBuildableMessage onLaunch, Func<SignedMessageTextUpdate, object>? parser)
            : this(property, u => onLaunch, parser) { }
        public InputSubProcess(PropertyInfo property, Func<ISignedUpdate, IBuildableMessage> onLaunch, Func<SignedMessageTextUpdate, object>? parser)
        {
            if (!typeof(T).GetProperties().Contains(property)) throw new ArgumentException(nameof(property));
            HandlingProperty = property;
            OnLaunch = onLaunch;
            ParseInput = parser ?? (x => x.Text);
        }

        public IRunningSubProcess<PartialInputRunning<T>> GetRunning(PartialInputRunning<T> owner)
        {
            var res = new InputSubProcessRunning<T>(owner, this)
            {
                InputPreview = InputPreview,
            };
            return res;
        }

        public override string ToString() => $"Prop input: {HandlingProperty.DeclaringType?.Name}.{HandlingProperty.Name}";
    }

    public class InputSubProcessRunning<T> : InputSubProcess<T>, IRunningSubProcess<PartialInputRunning<T>>
    {
        public PartialInputRunning<T> Owner { get; set; }
        public T BuildingInstance => Owner.BuildingInstance;

        public InputSubProcessRunning(PartialInputRunning<T> owner, InputSubProcess<T> launcher)
            : base(launcher.HandlingProperty, launcher.OnLaunch, launcher.ParseInput)
        {
            Owner = owner;
        }

        public async Task HandleInput(SignedMessageTextUpdate update)
        {
            var excepMes = InputPreview?.Invoke(update);
            if (excepMes is null)
            {
                HandlingProperty.SetValue(BuildingInstance, ParseInput(update));
                await Owner.Valid(update);
            }
            else
            {
                await update.Owner.DeliveryService.ReplyToSender(excepMes, update);
            }
        }
    }
}