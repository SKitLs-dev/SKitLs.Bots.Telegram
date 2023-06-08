using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model
{
    /// <summary>
    /// Delegate that represents <see cref="IBotAction{TUpdate}"/> holding async action.
    /// </summary>
    /// <typeparam name="TUpdate">Scecific casted update that this action should work with.</typeparam>
    /// <param name="update">An incoming update</param>
    public delegate Task BotInteraction<TUpdate>(TUpdate update) where TUpdate : ICastedUpdate;
}