using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMCProtocol
{
    internal sealed class PrivateImplementationDetails
    {
        internal static uint ComputeStringHash(string s)
        {
            uint num = 0u;
            if (s != null)
            {
                num = 2166136261u;
                for (int i = 0; i < s.Length; i++)
                {
                    num = (s[i] ^ num) * 16777619;
                }
            }

            return num;
        }
    }
}
