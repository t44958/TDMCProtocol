using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMCProtocol
{
 
    public interface IProtocol : IDisposable
    {
        int Port { get; }

        string Ip { get; }

        bool IsConnected { get; }

        bool IsAvailable { get; }

        event ProtocolExceptionEventHandler ProtocolException;

        bool Open();

        void Close();

        void Write(string variableName, VarType varType, ushort varCount, object dataToWrite);

        void Write(string variableName, VarType varType, object dataToWrite);

        object Read(string variableName, VarType varType, ushort varCount);

        object Read(string variableName, VarType varType);
    }
}
