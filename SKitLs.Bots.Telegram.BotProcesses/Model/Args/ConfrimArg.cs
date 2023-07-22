using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Model.DefaultProcesses;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Args
{
    public class ConfrimArg<TUpate> where TUpate : ICastedUpdate, ISignedUpdate
    {
        [BotActionArgument(0)]
        public bool Confirm { get; set; }

        [BotActionArgument(2)]
        public YesNoProcess<TUpate> Process { get; set; } = null!;

        public ConfrimArg() { }
        public ConfrimArg(bool cnf, YesNoProcess<TUpate> procId)
        {
            Confirm = cnf;
            Process = procId;
        }

        //public IBotDisplayable GetObject() => DataSet.GetData(ObjId);
    }
}