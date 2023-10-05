using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Messages
{
    internal class ObjMenu<T> : IPageMenu where T : class, IBotDisplayable
    {
        public IDataManager Owner { get; private set; }
        public ObjInfoArg ObjInfo { get; private set; }
        public IBotDisplayable Object => ObjInfo.GetObject();

        public ObjMenu(IDataManager owner, ObjInfoArg infoArg)
        {
            Owner = owner;
            ObjInfo = infoArg;
        }

        public async Task<IBuildableContent<IMessageMenu>> BuildAsync(IBotPage? previous, IBotPage owner, ISignedUpdate update)
        {
            if (ObjInfo.DataSet.DataType != typeof(T))
                throw new Exception("ObjMenu => types miss match");
            var result = new InlineMenu(update.Owner);

            var ds = (IBotDataSet<T>)ObjInfo.DataSet;
            if (ds.Properties.AllowEdit)
                result.Add(Owner.EditExistingCallback, ObjInfo);
            if (ds.Properties.AllowRemove)
                result.Add(Owner.RemoveExistingCallback, ObjInfo);
            ds.GetObjectActions().ForEach(action => result.Add(action, new DtoArg<T>(ObjInfo.GetObject<T>(), ds)));
            result.Add("<< Назад", Owner.OpenDatabaseCallback, ObjInfo.GetPagination());

            return await Task.FromResult(result);
        }
    }
}