using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype
{
    /// <summary>
    /// Provides mechanics of realization specific bot actions <see cref="IBotAction{TUpdate}"/> that contains arguments.
    /// Arguments are being deserialized and unpacked during runtime automatically.
    /// </summary>
    /// <typeparam name="TArg"></typeparam>
    /// <typeparam name="TUpdate"></typeparam>
    public interface IArgedAction<TArg, TUpdate> : IBotAction<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// Represents specific token that action's data is split with.
        /// <para>
        /// Example: <c>';'</c> token by default for <see cref="BotArgedCallback{TArg}"/> so its data is
        /// <c>'callbackName;arg1;arg2;arg3'</c>.
        /// </para>
        /// </summary>
        public char SplitToken { get; }

        /// <summary>
        /// An action that should be executed on update.
        /// </summary>
        public BotArgedInteraction<TArg, TUpdate> ArgAction { get; }

        /// <summary>
        /// Deserializes an incoming string data to get a specific <typeparamref name="TArg"/> instance with
        /// unpacked received data.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <param name="serializer">Bot's serialization service.</param>
        /// <returns>Result of converting attempt.</returns>
        public ConvertResult<TArg> DeserializeArgs(TUpdate update, IArgsSerializeService serializer);

        /// <summary>
        /// Serializes <paramref name="data"/>'s properties marked with <see cref="BotActionArgumentAttribute"/>
        /// with rules provided by <paramref name="serializer"/>.
        /// </summary>
        /// <param name="data">An argument object that should be serialized.</param>
        /// <param name="serializer">Bot's serialization service.</param>
        /// <returns>Serialized <paramref name="data"/>.</returns>
        public string SerializeArgs(TArg data, IArgsSerializeService serializer);

        /// <summary>
        /// Generates ready-to-use <see cref="string"/>.
        /// </summary>
        /// <param name="data">An argument object that should be serialized.</param>
        /// <param name="serializer">Bot's serialization service.</param>
        /// <returns>Ready-to-use <see cref="string"/> that can be used as actor.</returns>
        public string GetSerializedData(TArg data, IArgsSerializeService serializer);
    }
}