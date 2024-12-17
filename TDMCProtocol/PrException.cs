using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMCProtocol
{
    public class PrException : Exception
    {
        public ExceptionCode ExCode { get; }

        public PrException(ExceptionCode exCode)
            : base((byte)exCode + "-" + exCode)
        {
            ExCode = exCode;
        }

        public PrException(ExceptionCode exCode, string AddInfo)
            : base(AddInfo + ": " + (byte)exCode + "-" + exCode)
        {
            ExCode = exCode;
        }
    }
}
