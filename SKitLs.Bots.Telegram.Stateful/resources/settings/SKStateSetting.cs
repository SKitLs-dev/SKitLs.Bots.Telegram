using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKitLs.Bots.Telegram.Stateful.resources.settings
{
    /// <summary>
    /// The <see cref="SKStateSetting"/> class provides settings related to the SKitLs.Bots.Telegram.Stateful extension.
    /// </summary>
    public class SKStateSetting
    {
        /// <summary>
        /// Gets or sets the prefix used for the extension's localizations.
        /// Default value is "state".
        /// </summary>
        public static string ExtensionPrefix { get; set; } = "state";
    }
}