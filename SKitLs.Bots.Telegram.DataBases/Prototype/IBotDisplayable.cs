namespace SKitLs.Bots.Telegram.DataBases.Prototype
{
    public interface IBotDisplayable
    {
        public long BotArgId { get; }

        public void UpdateId(long id);
        /// <summary>
        /// Used to display full object information
        /// </summary>
        /// <param name="args">.</param>
        /// <returns></returns>
        public string FullDisplay(params string[] args);
        /// <summary>
        /// Represents the way how object should be displayed in a list representation.
        /// </summary>
        /// <returns></returns>
        public string ListDisplay();
        /// <summary>
        /// Represents the way how object should be displayed in a list representation.
        /// </summary>
        /// <returns></returns>
        public string ListLabel();
    }
}