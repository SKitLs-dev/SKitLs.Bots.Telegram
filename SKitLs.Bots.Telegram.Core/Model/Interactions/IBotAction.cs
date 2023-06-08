namespace SKitLs.Bots.Telegram.Core.Model.Interactions
{
    /// <summary>
    /// An interface that provides generic defenition for bot actions.
    /// <para>
    /// Fifth architecture level.
    /// Upper: <see cref="Management.IActionManager{TUpdate}"/>.
    /// </para>
    /// </summary>
    public interface IBotAction
    {
        /// <summary>
        /// Unique action's ID.
        /// </summary>
        public string ActionId { get; }

        /// <summary>
        /// Gets serialized data that can be built with certain arguments.
        /// Ready to use
        /// </summary>
        /// <param name="args">Arguments to be used</param>
        /// <returns>Ready to use string data.</returns>
        public string GetSerializedData(params string[] args);
    }
}