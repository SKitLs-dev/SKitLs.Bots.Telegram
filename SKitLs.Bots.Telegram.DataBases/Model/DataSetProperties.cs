using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model
{
    public class DataSetProperties
    {
        /// <summary>
        /// Data set's display label
        /// </summary>
        public string DataSetLabel { get; }
        public int PaginationCount { get; set; }
        public IOutputMessage? MainPageBody { get; set; }
        public bool AllowReadRows { get; set; } = true;
        public bool AllowAdd { get; set; } = true;
        public bool ConfirmAdd { get; set; } = true;
        public bool AllowEdit { get; set; } = true;
        public bool ConfirmEdit { get; set; } = true;
        public bool AllowRemove { get; set; } = true;
        public bool ConfirmRemove { get; set; } = true;
        internal bool AllowExit { get; set; } = true;

        public DataSetProperties(string dataSetLabel, int paginationCount = 5, IOutputMessage? mainPageBody = null)
        {
            DataSetLabel = dataSetLabel ?? throw new ArgumentNullException(nameof(dataSetLabel));
            PaginationCount = paginationCount;
            MainPageBody = mainPageBody;
        }
    }
}