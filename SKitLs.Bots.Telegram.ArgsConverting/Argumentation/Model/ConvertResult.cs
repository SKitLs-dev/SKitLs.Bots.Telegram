using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model
{
    public abstract class ConvertResult
    {
        /// <summary>
        /// Represents the targeted result type for the conversion.
        /// </summary>
        public abstract Type ValueType { get; }

        /// <summary>
        /// Represents the type of the conversion result.
        /// </summary>
        public virtual ConvertResultType ResultType { get; private init; }

        /// <summary>
        /// Contains a message that describes the conversion result.
        /// </summary>
        public string ResultMessage { get; private init; }

        public ConvertResult(ConvertResultType resultType, string? message = null)
        {
            ResultType = resultType;
            ResultMessage = message ?? Enum.GetName(resultType) ?? "Unknown";
        }

        public abstract object GetValue();
    }
}