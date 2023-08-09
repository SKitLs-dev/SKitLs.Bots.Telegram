using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype
{
    /// <summary>
    /// Provides mechanics for implementing specific bot actions <see cref="IBotAction{TUpdate}"/> that involve arguments.
    /// Arguments are automatically deserialized and unpacked during runtime.
    /// </summary>
    /// <typeparam name="TArg">The type representing the action's arguments.</typeparam>
    /// <typeparam name="TUpdate">The type of the incoming update.</typeparam>
    public interface IArgedAction<TArg, TUpdate> : IBotAction<TUpdate> where TUpdate : ICastedUpdate where TArg : notnull
    {
        /// <summary>
        /// Represents the specific token that the action's data is split with.
        /// </summary>
        public char SplitToken { get; }

        /// <summary>
        /// The action that should be executed on the update.
        /// </summary>
        public BotArgedInteraction<TArg, TUpdate> ArgAction { get; }

        /// <summary>
        /// Deserializes the incoming string data to obtain a specific <typeparamref name="TArg"/> instance
        /// with unpacked received data.
        /// </summary>
        /// <param name="update">The incoming update.</param>
        /// <param name="serializer">The bot's serialization service.</param>
        /// <returns>The result of the conversion attempt.</returns>
        public ConvertResult<TArg> DeserializeArgs(TUpdate update, IArgsSerializeService serializer);

        /// <summary>
        /// Serializes the properties of <paramref name="data"/> marked with <see cref="BotActionArgumentAttribute"/>
        /// using the rules provided by <paramref name="serializer"/>.
        /// </summary>
        /// <param name="data">The argument object to be serialized.</param>
        /// <param name="serializer">The bot's serialization service.</param>
        /// <returns>The serialized <paramref name="data"/>.</returns>
        public string SerializeArgs(TArg data, IArgsSerializeService serializer);

        /// <summary>
        /// Generates a ready-to-use <see cref="string"/>.
        /// </summary>
        /// <param name="data">The argument object to be serialized.</param>
        /// <param name="serializer">The bot's serialization service.</param>
        /// <returns>A ready-to-use <see cref="string"/> that can be used as an actor.</returns>
        public string GetSerializedData(TArg data, IArgsSerializeService serializer);
    }
}