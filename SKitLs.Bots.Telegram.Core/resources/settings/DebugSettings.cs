namespace SKitLs.Bots.Telegram.Core.resources.Settings
{
    public class DebugSettings
    {
        private DebugSettings() { }
        public static DebugSettings Default() => new();

        #region Bot Manager
        public bool ShouldPrintUpdates { get; set; } = true;
        public bool ShouldPrintExceptions { get; set; } = true;

        public bool Nfy_ChatNotHandled { get; set; } = true;
        public bool Nfy_ChatIdNotHandled { get; set; } = true;
        public bool Nfy_ChatTypeNotSupported { get; set; } = true;

        #endregion

        //public string? ChatNotHandledMessage { get; set; } = $"{nameof(DebugSettings)}-" +
        //    $"{nameof(NotificateChatNotHandled)}: Не удалось определить тип чата";
        //public UpdateRelatedAsyncTask? ChatNotHandledAction { get; set; }

        //public string? ChatIdNotHandledMessage { get; set; } = $"{nameof(DebugSettings)}-" +
        //    $"{nameof(NotificateChatIdNotHandled)}: Не удалось определить ID чата";
        //public UpdateRelatedAsyncTask? ChatIdNotHandled { get; private set; }

    }
}
