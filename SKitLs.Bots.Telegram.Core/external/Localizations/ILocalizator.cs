using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKitLs.Bots.Telegram.Core.external.Localizations
{
    public interface ILocalizator
    {
        public string LocalsPath { get; }

        public string ResolveString(LangKey lang, string key, params string?[] format);
    }
}
