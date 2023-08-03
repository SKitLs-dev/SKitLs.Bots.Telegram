using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SKitLs.Bots.Telegram.BotProcesses.Prototype
{
    internal interface IMaskedInput
    {
        /// <summary>
        /// Determines special string mask that would be used to unpack input text.
        /// </summary>
        public string Mask { get; }

        /// <summary>
        /// Uses <see cref="Mask"/> to unpack input string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string Demask(string input)
        {
            if (Mask is null) return input;
            string pattern = Mask.Replace("{0}", "(.*?)");
            var match = Regex.Match(input, pattern);
            if (!match.Success || match.Groups.Count < 2) throw new Exception();
            return match.Groups[1].Value;
        }
    }
}