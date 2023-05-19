using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model
{
    public class BotDataSet : IBotDataSet
    {
        public string SetId { get; set; }
        public string BotArgId { get; set; }

        public string SetLabel { get; set; }
        public string DisplayName { get; set; }
        public IOutputMessage? MainPageBody { get; set; }

        public List<IBotDisplayable> Data { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public Type DataType { get; set; }

        public BotDataSet(string id, List<IBotDisplayable> data)
        {
            SetId = id;
            Data = data;
            SetLabel = id;
            DisplayName = id;

            PaginationInfo = new(this, 0, 5);
        }

        public string FullDisplay(int fullness = 0)
        => nameof(FullDisplay);

        public string ListDisplay() => DisplayName;
        public string ListLabel() => SetLabel;

        public string ShortDisplay() => nameof(ShortDisplay);

        public string GetPacked() => SetId;

        public IBotDisplayable GetNew()
        {
            throw new NotImplementedException();
        }

        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public void Subscribe()
        {
            throw new NotImplementedException();
        }

        public string FullDisplay(params string[] args)
        {
            throw new NotImplementedException();
        }
    }
}