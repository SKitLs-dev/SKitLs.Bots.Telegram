using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKitLs.Bots.Telegram.Core.external.Localizations
{
    public class Localizator : ILocalizator
    {
        public string LocalsPath => throw new NotImplementedException();

        public string ResolveString(LangKey lang, string key, params string?[] format)
        {
            return string.Empty;
        }
    }
}
