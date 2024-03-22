using SKitLs.Bots.Telegram.Core.Model.Management;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions
{
    /// <summary>
    /// Represents an interface that provides a generic definition for bot actions.
    /// <para/>
    /// This interface represents the <b>fifth level</b> of the bot's architecture.
    /// <list type="number">
    ///     <item/>
    ///     <item/>
    ///     <item/>
    ///     <item>
    ///         <term><see cref="IActionManager{TUpdate}"/></term>
    ///         <description>Provides complex logic for handling updates via interactions.</description>
    ///     </item>
    ///     <item>
    ///         <b><see cref="IBotAction"/></b>
    ///     </item>
    /// </list>
    /// </summary>
    public interface IBotAction
    {
        /// <summary>
        /// Gets the unique identifier of the action.
        /// </summary>
        public string ActionId { get; }

        /// <summary>
        /// Gets serialized data that can be built with certain arguments, making it ready-to-use.
        /// </summary>
        /// <param name="args">Arguments to be used in constructing the serialized data.</param>
        /// <returns>Serialized data ready for use.</returns>
        public string GetSerializedData(params string[] args);
    }
}