using System.Text.RegularExpressions;

namespace SKitLs.Bots.Telegram.BotProcesses.Prototype
{
    /// <summary>
    /// Represents an interface for working with masked input, allowing unpacking input using a specified mask.
    /// </summary>
    public interface IMaskedInput
    {
        /// <summary>
        /// Represents the special string mask that is used to unpack input text.
        /// </summary>
        public string Mask { get; }

        /// <summary>
        /// Unpacks the input string using the specified <see cref="Mask"/>.
        /// </summary>
        /// <param name="input">The input string to be demasked.</param>
        /// <returns>The demasked value extracted from the input, based on the specified mask.</returns>
        /// <exception cref="Exception">Thrown when the demasking process fails, typically due to an invalid mask or pattern.</exception>
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