﻿using SKitLs.Bots.Telegram.Core.Prototypes;

namespace SKitLs.Bots.Telegram.Core.Model.UpdatesCasting
{
    /// <summary>
    /// Represents a signed update. Determines update that contains specific <see cref="IBotUser"/> sender.
    /// </summary>
    public interface ISignedUpdate : ICastedUpdate
    {
        /// <summary>
        /// Casted sender instance that has raised an update.
        /// <para>
        /// Generates via <see cref="ChatScanner.UsersManager"/>
        /// or <see cref="ChatScanner.GetDefaultBotUser"/> mechanisms
        /// of a <see cref="ChatScanner"/> class by default.
        /// </para>
        /// </summary>
        public IBotUser Sender { get; }
    }
}