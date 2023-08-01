using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model
{
    /// <summary>
    /// Represents <see cref="IBotAction{TUpdate}"/> holding async action.
    /// </summary>
    /// <typeparam name="TUpdate">Specific casted update that this action should work with.</typeparam>
    /// <param name="update">An incoming update.</param>
    public delegate Task BotInteraction<TUpdate>(TUpdate update) where TUpdate : ICastedUpdate;

    /// <summary>
    /// Represents specified async delegate that gets <see cref="IBotUser"/> as a parameter.
    /// </summary>
    /// <param name="user">User that has been modified.</param>
    public delegate Task UserDataChanged(IBotUser user);
}