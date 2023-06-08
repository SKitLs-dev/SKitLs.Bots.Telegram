using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Anonim;
using SKitLs.Bots.Telegram.Core.Prototypes;
using Telegram.Bot.Types;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed
{
    /// <summary>
    /// Casted update that represents default signed message update.
    /// Signed variants of a <see cref="Anonim.AnonimMessageUpdate"/>
    /// </summary>
    public class SignedMessageUpdate : AnonimMessageUpdate, ISignedUpdate
    {
        public IBotUser Sender { get; private set; }

        /// <summary>
        /// Creates a new instance of a <see cref="SignedMessageUpdate"/>, using specific data.
        /// </summary>
        /// <param name="chatScanner">Chat Scanner that has raised casted update</param>
        /// <param name="source">Original telegram update. Not casted, contains null values</param>
        /// <param name="chatId">ID of a chat that has raised updated</param>
        /// <param name="sender">Casted sender instance that has raised an update</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedMessageUpdate(ChatScanner chatScanner, Update source, long chatId, IBotUser sender)
            : base(chatScanner, source, chatId) => Sender = sender ?? throw new NullSenderException();

        /// <summary>
        /// Creates a new instance of a <see cref="SignedMessageUpdate"/>,
        /// coping data from other <see cref="ICastedUpdate"/> and signing it with
        /// <paramref name="sender"/> instance.
        /// </summary>
        /// <param name="update">An instance to be copied</param>
        /// <param name="sender">Casted sender instance that has raised an update</param>
        /// <exception cref="UpdateCastingException"></exception>
        /// <exception cref="NullSenderException"></exception>
        public SignedMessageUpdate(ICastedUpdate update, IBotUser sender) : this(update.ChatScanner, update.OriginalSource, update.ChatId, sender) { }
    }
}