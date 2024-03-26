using SKitLs.Bots.Telegram.AdvancedMessages.Messages;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.PageNavs.Pages;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <summary>
    /// Represents a delegate that asynchronously returns a string based on a menu page and a signed update.
    /// </summary>
    /// <param name="page">The menu page.</param>
    /// <param name="update">The signed update.</param>
    /// <returns>A task representing the asynchronous operation, returning a string.</returns>
    public delegate Task<string> DynamicPageStringTask(IBotPage page, ISignedUpdate update);

    /// <summary>
    /// Represents a delegate that asynchronously returns an array of strings based on a menu page and a signed update.
    /// </summary>
    /// <param name="page">The menu page.</param>
    /// <param name="update">The signed update.</param>
    /// <returns>A task representing the asynchronous operation, returning an array of strings.</returns>
    public delegate Task<string[]> DynamicPageStringsTask(IBotPage page, ISignedUpdate update);

    /// <summary>
    /// Represents a delegate that asynchronously returns an output message based on a menu page and a signed update.
    /// </summary>
    /// <param name="page">The menu page.</param>
    /// <param name="update">The signed update.</param>
    /// <returns>A task representing the asynchronous operation, returning an output message.</returns>
    public delegate Task<IOutputMessage> DynamicPageMessageTask(IBotPage page, ISignedUpdate update);
}