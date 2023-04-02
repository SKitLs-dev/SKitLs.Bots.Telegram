using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    public class NullSenderException : SKTgException
    {
        public NullSenderException() : base(true, "exception_NullSender")
        { }
    }
}
