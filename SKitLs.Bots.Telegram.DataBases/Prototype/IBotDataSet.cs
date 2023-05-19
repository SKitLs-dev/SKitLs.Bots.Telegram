using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;
using SKitLs.Bots.Telegram.DataBases.Model.Args;

namespace SKitLs.Bots.Telegram.DataBases.Prototype
{
    public interface IBotDataSet : IArgPackable, IBotDisplayable
    {
        public PaginationInfo PaginationInfo { get; }
        public string SetId { get; }
        public string SetLabel { get; }
        public IOutputMessage MainPageBody { get; }
        public List<IBotDisplayable> Data { get; }
        public IBotDisplayable GetData(string bid) => Data.Find(x => x.BotArgId == bid) ?? throw new Exception();

        public Type DataType { get; }
        public IBotDisplayable GetNew();
        public void Add();
        public void Remove(string id);
        public void Subscribe();
    }
}