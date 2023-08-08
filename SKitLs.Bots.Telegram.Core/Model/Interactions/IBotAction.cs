namespace SKitLs.Bots.Telegram.Core.Model.Interactions
{
    // XML-Doc Update
    /// <summary>
    /// An interface that provides generic definition for bot actions.
    /// <para>
    /// Fifth architecture level.
    /// Upper: <see cref="Management.IActionManager{TUpdate}"/>.
    /// </para>
    /// </summary>
    public interface IBotAction
    {
        /// <summary>
        /// Represents the unique identifier of the action.
        /// </summary>
        public string ActionId { get; }

        /// <summary>
        /// Gets serialized data that can be built with certain arguments.
        /// Ready-to-use.
        /// </summary>
        /// <param name="args">Arguments to be used.</param>
        /// <returns>Ready to use string data.</returns>
        public string GetSerializedData(params string[] args);
    }
}