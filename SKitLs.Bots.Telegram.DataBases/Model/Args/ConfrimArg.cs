using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.DataBases.external;
using SKitLs.Bots.Telegram.DataBases.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Args
{
    public class ConfrimArg<TUpate> where TUpate : ICastedUpdate, ISignedUpdate
    {
        [BotActionArgument(0)]
        public bool Confirm { get; set; }

        [BotActionArgument(2)]
        public YesNoDataProcess<TUpate> Process { get; set; } = null!;

        public ConfrimArg() { }
        public ConfrimArg(bool cnf, YesNoDataProcess<TUpate> procId)
        {
            Confirm = cnf;
            Process = procId;
        }

        //public IBotDisplayable GetObject() => DataSet.GetData(ObjId);
    }
}