using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMCProtocol
{
    public enum VarType
    {
        Bit,
        Byte,
        SByte,
        Word,
        DWord,
        Int,
        DInt,
        Float,
        Double,
        String,
        Timer,
        Counter
    }
    public delegate void ProtocolExceptionEventHandler(object sender, ExceptionCode exCode, string information);

    public enum ExceptionCode
    {
        Normal = 0,
        ReadDataFailt = 1,
        WriteDataFailt = 2,
        WrongVariableFormat = 3,
        DataToWriteInvalid = 4,
        ResponseAbnormalCompletion = 5,
        VarTypeNotSupport = 6,
        WrongCPU_Type = 7,
        TagCollectionEmpty = 32,
        TagSetupInvalid = 33,
        TagCollectionInitFailt = 34,
        TagNameNotExist = 35,
        ConnectionFailt = 12,
        DataConversionFault = 15,
        Unknown = 255
    }


    public enum DeviceName
    {
        SpecialRelay = 145,
        SpecialRegister = 169,
        Input = 156,
        Output = 157,
        InternalRelay = 144,
        LatchRelay = 146,
        Annunciator = 147,
        EdgeRelay = 148,
        LinkRelay = 160,
        DataRegister = 168,
        LinkRegister = 180,
        Timer = 194,
        STimer = 200,
        Counter = 197,
        LinkSpecialRelay = 161,
        LinkSpecialRegister = 181,
        DirectAccessInput = 162,
        DirectAccessOutput = 163,
        IndexRegister = 204
    }
}
