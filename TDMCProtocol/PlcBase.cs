//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TDMCProtocol
//{
//    internal class PlcBase
//    {
//    }
//}

using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
//using NMCProtocol.DataPLC;


namespace TDMCProtocol
{

public abstract class PlcBase : IProtocol, IDisposable
{
    protected enum Command
    {
        Read = 1025,
        Write = 5121
    }

    protected enum Units
    {
        Word,
        Bit
    }

    protected Socket _mSocket;

    public int Port { get; private set; }

    public string Ip { get; private set; }

    public string LastErrorString => LastErrorCode + "-" + (ExceptionCode)LastErrorCode;

    public byte LastErrorCode { get; protected set; }

    public bool IsConnected
    {
        get
        {
            try
            {
                if (_mSocket == null)
                {
                    return false;
                }

                return (!_mSocket.Poll(1000, SelectMode.SelectRead) || _mSocket.Available != 0) && _mSocket.Connected;
            }
            catch
            {
                return false;
            }
        }
    }

    public bool IsAvailable
    {
        get
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                return Connect(socket);
        }
    }

    //public string Product => Security.GetDriveSerialNumber();

    //public string License { get; set; }

    //public bool ProductKeyApplied => License == Security.Hash(Product, "[AUTOMATION_SW]-MC.PROTOCOL", new SHA1CryptoServiceProvider());

    public event ProtocolExceptionEventHandler ProtocolException;

    private void TrialExpired(object sender, EventArgs e)
    {
        Close();
    }

    public PlcBase(string ip, int port)
    {
        Ip = ip;
        Port = port;
    }

    public virtual bool Open()
    {
        if (IsConnected)
        {
            return IsConnected;
        }

        _mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _mSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
        _mSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1000);
        if (Connect(_mSocket))
        {
            return true;
        }

        LastErrorCode = 12;
        return false;
    }

    public void Close()
    {
        if (_mSocket != null && _mSocket.Connected)
        {
            _mSocket.Shutdown(SocketShutdown.Both);
            _mSocket.Close();
        }
    }

    public void Dispose()
    {
        if (_mSocket != null && _mSocket.Connected)
        {
            _mSocket.Shutdown(SocketShutdown.Both);
            _mSocket.Close();
        }
    }

    protected abstract object Read(DeviceName deviceName, uint deviceNumber, VarType varType, ushort varCount);

    public abstract object Read(string variableName, VarType varType, ushort varCount);

    public abstract object Read(string variableName, VarType varType);

    protected abstract void Write(DeviceName deviceName, uint deviceNumber, VarType varType, ushort varCount, object dataToWrite);

    public abstract void Write(string variableName, VarType varType, ushort varCount, object dataToWrite);

    public abstract void Write(string variableName, VarType varType, object dataToWrite);

    protected abstract byte[] DeviceNameToBytes(DeviceName deviceName);

    protected abstract byte[] DeviceNumberToBytes(uint deviceNumber);

    protected abstract byte[] SubCommadToBytes(Units units);

    protected abstract byte[] CreateReadDataRequestPackage(DeviceName deviceName, uint deviceNumber, Units units, ushort count = 1);

    protected abstract byte[] CreateWriteDataRequestPackage(DeviceName deviceName, uint deviceNumber, Units units, ushort count, byte[] data);

    protected abstract byte[] ReadBytesWithASingleRequest(DeviceName device, uint deviceNumber, Units units, ushort count);

    protected abstract void WriteBytesWithASingleRequest(DeviceName device, uint deviceNumber, Units units, ushort count, byte[] data);

    protected abstract uint GetDeviceNumber(string variableName);

    protected virtual void OnProtocolException(object sender, ExceptionCode exCode, string exInfomation)
    {
        if (this.ProtocolException != null)
        {
            this.ProtocolException(sender, exCode, exInfomation);
        }
    }

    private bool Connect(Socket socket)
    {
        //if (Security.TrialExpired)
        //{
        //    return false;
        //}

        //if (!ProductKeyApplied)
        //{
        //    Security security = new Security(1800);
        //    security.TrialEvent += TrialExpired;
        //    security.Run();
        //}

        try
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(Ip), Port);
            socket.Connect(remoteEP);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected virtual object ParseBytes(VarType varType, byte[] bytes, ushort varCount = 1)
    {
        if (bytes == null)
        {
            return GetDefault(varType, varCount);
        }

        try
        {
            switch (varType)
            {
                case VarType.Bit:
                    if (varCount == 1)
                    {
                        return Conversion.ToBoolean(bytes);
                    }

                    return Conversion.ToBooleanArray(bytes, varCount);
                case VarType.Byte:
                    if (varCount == 1)
                    {
                        return Conversion.ToByte(bytes);
                    }

                    return Conversion.ToByteArray(bytes);
                case VarType.SByte:
                    if (varCount == 1)
                    {
                        return Conversion.ToSByte(bytes);
                    }

                    return Conversion.ToSByteArray(bytes);
                case VarType.Word:
                    if (varCount == 1)
                    {
                        return Conversion.ToUInt16(bytes);
                    }

                    return Conversion.ToUInt16Array(bytes);
                case VarType.DWord:
                    if (varCount == 1)
                    {
                        return Conversion.ToUInt32(bytes);
                    }

                    return Conversion.ToUInt32Array(bytes);
                case VarType.Int:
                case VarType.Timer:
                case VarType.Counter:
                    if (varCount == 1)
                    {
                        return Conversion.ToInt16(bytes);
                    }

                    return Conversion.ToInt16Array(bytes);
                case VarType.DInt:
                    if (varCount == 1)
                    {
                        return Conversion.ToInt32(bytes);
                    }

                    return Conversion.ToInt32Array(bytes);
                case VarType.Float:
                    if (varCount == 1)
                    {
                        return Conversion.ToSingle(bytes);
                    }

                    return Conversion.ToSingleArray(bytes);
                case VarType.Double:
                    if (varCount == 1)
                    {
                        return Conversion.ToDouble(bytes);
                    }

                    return Conversion.ToDoubleArray(bytes);
                case VarType.String:
                    if (varCount == 1)
                    {
                        return Conversion.ToString(bytes);
                    }

                    return Conversion.ToString(bytes);
                default:
                    return GetDefault(varType, varCount);
            }
        }
        catch
        {
            throw new PrException(ExceptionCode.DataConversionFault);
        }
    }

    protected virtual object GetDefault(VarType varType, ushort varCount = 1)
    {
        switch (varType)
        {
            case VarType.Bit:
                if (varCount > 1)
                {
                    return new bool[1];
                }

                return false;
            case VarType.Byte:
                if (varCount > 1)
                {
                    return new byte[1];
                }

                return 0;
            case VarType.SByte:
                if (varCount > 1)
                {
                    return new sbyte[1];
                }

                return 0;
            case VarType.Word:
                if (varCount > 1)
                {
                    return new ushort[1];
                }

                return 0;
            case VarType.DWord:
                if (varCount > 1)
                {
                    return new uint[1];
                }

                return 0u;
            case VarType.Int:
            case VarType.Timer:
            case VarType.Counter:
                if (varCount > 1)
                {
                    return new short[1];
                }

                return 0;
            case VarType.DInt:
                if (varCount > 1)
                {
                    return new int[1];
                }

                return 0;
            case VarType.Float:
                if (varCount > 1)
                {
                    return new float[1];
                }

                return 0f;
            case VarType.Double:
                if (varCount > 1)
                {
                    return new double[1];
                }

                return 0.0;
            case VarType.String:
                return "";
            default:
                return null;
        }
    }

    protected virtual ushort NumberOfDevicePoints(VarType varType, ushort varCount = 1)
    {
        switch (varType)
        {
            case VarType.Bit:
            case VarType.Byte:
            case VarType.SByte:
            case VarType.Word:
            case VarType.Int:
            case VarType.String:
            case VarType.Timer:
            case VarType.Counter:
                return varCount;
            case VarType.DWord:
            case VarType.DInt:
            case VarType.Float:
                return (ushort)(varCount * 2);
            case VarType.Double:
                return (ushort)(varCount * 4);
            default:
                return 0;
        }
    }

    protected virtual byte[] CommandToBytes(Command cmd)
    {
        return BitConverter.GetBytes((short)(ushort)cmd);
    }

    protected virtual DeviceName GetDeviceName(string variableName)
    {
        variableName = variableName.Replace(" ", "");
        variableName = variableName.ToUpper();
        string text = variableName.Substring(0, 2);
        switch (PrivateImplementationDetails.ComputeStringHash(text))
        {
            case 1426227232u:
                if (text == "SB")
                {
                    return DeviceName.LinkSpecialRelay;
                }

                break;
            case 818834449u:
                if (text == "DX")
                {
                    return DeviceName.DirectAccessInput;
                }

                break;
            case 802056830u:
                if (text == "DY")
                {
                    return DeviceName.DirectAccessOutput;
                }

                break;
            case 1677891517u:
                if (text == "SM")
                {
                    return DeviceName.SpecialRelay;
                }

                break;
            case 1526892946u:
                if (text == "SD")
                {
                    return DeviceName.SpecialRelay;
                }

                break;
            case 1795334850u:
                if (text == "ST")
                {
                    return DeviceName.STimer;
                }

                break;
            case 1778557231u:
                if (text == "SW")
                {
                    return DeviceName.LinkSpecialRegister;
                }

                break;
        }

        string text2 = variableName.Substring(0, 1);
        switch (PrivateImplementationDetails.ComputeStringHash(text2))
        {
            case 3322673650u:
                if (text2 == "C")
                {
                    return DeviceName.Counter;
                }

                break;
            case 3272340793u:
                if (text2 == "F")
                {
                    return DeviceName.Annunciator;
                }

                break;
            case 3238785555u:
                if (text2 == "D")
                {
                    return DeviceName.DataRegister;
                }

                break;
            case 3373006507u:
                if (text2 == "L")
                {
                    return DeviceName.LatchRelay;
                }

                break;
            case 3356228888u:
                if (text2 == "M")
                {
                    return DeviceName.InternalRelay;
                }

                break;
            case 3339451269u:
                if (text2 == "B")
                {
                    return DeviceName.LinkRelay;
                }

                break;
            case 3540782697u:
                if (text2 == "V")
                {
                    return DeviceName.EdgeRelay;
                }

                break;
            case 3524005078u:
                if (text2 == "W")
                {
                    return DeviceName.LinkRegister;
                }

                break;
            case 3507227459u:
                if (text2 == "T")
                {
                    return DeviceName.Timer;
                }

                break;
            case 3742114125u:
                if (text2 == "Z")
                {
                    return DeviceName.IndexRegister;
                }

                break;
            case 3708558887u:
                if (text2 == "X")
                {
                    return DeviceName.Input;
                }

                break;
            case 3691781268u:
                if (text2 == "Y")
                {
                    return DeviceName.Output;
                }

                break;
        }

        throw new PrException(ExceptionCode.WrongVariableFormat);
    }

    protected virtual byte[] ParseWriteData(byte[] bytes, Units units, ushort numberOfDevicePoints)
    {
        ushort writeDataLength = GetWriteDataLength(units, numberOfDevicePoints);
        ushort num = (ushort)bytes.Length;
        ByteArray byteArray = new ByteArray();
        byteArray.Add(bytes);
        for (ushort num2 = writeDataLength; num2 > num; num2--)
        {
            byteArray.Add(0);
        }

        for (ushort num3 = writeDataLength; num3 < num; num3++)
        {
            byteArray.Remove();
        }

        return byteArray.array;
    }

    protected virtual byte[] GetDataToWrite(object data)
    {
        string name = data.GetType().Name;
        switch (PrivateImplementationDetails.ComputeStringHash(name))
        {
            case 423635464u:
                if (name == "SByte")
                {
                    return Conversion.ToBytes((short)data);
                }

                break;
            case 238028739u:
                if (name == "Boolean[]")
                {
                    return Conversion.ToBytes((bool[])data);
                }

                break;
            case 765439473u:
                if (name == "Int16")
                {
                    return Conversion.ToBytes((short)data);
                }

                break;
            case 468055569u:
                if (name == "Single[]")
                {
                    return Conversion.ToBytes((float[])data);
                }

                break;
            case 1323747186u:
                if (name == "UInt16")
                {
                    return Conversion.ToBytes((ushort)data);
                }

                break;
            case 1189326818u:
                if (name == "UInt16[]")
                {
                    return Conversion.ToBytes((ushort[])data);
                }

                break;
            case 2341828857u:
                if (name == "Int16[]")
                {
                    return Conversion.ToBytes((short[])data);
                }

                break;
            case 2313474264u:
                if (name == "UInt32[]")
                {
                    return Conversion.ToBytes((uint[])data);
                }

                break;
            case 1615808600u:
                if (name == "String")
                {
                    return Conversion.ToBytes((string)data);
                }

                break;
            case 2642490659u:
                if (name == "Byte[]")
                {
                    return Conversion.ToBytes((ushort[])data);
                }

                break;
            case 2386971688u:
                if (name == "Double")
                {
                    return Conversion.ToBytes((double)data);
                }

                break;
            case 3509231420u:
                if (name == "Double[]")
                {
                    return Conversion.ToBytes((double[])data);
                }

                break;
            case 3409549631u:
                if (name == "Byte")
                {
                    return Conversion.ToBytes((ushort)data);
                }

                break;
            case 2711245919u:
                if (name == "Int32")
                {
                    return Conversion.ToBytes((int)data);
                }

                break;
            case 3646816451u:
                if (name == "Int32[]")
                {
                    return Conversion.ToBytes((int[])data);
                }

                break;
            case 3538687084u:
                if (name == "UInt32")
                {
                    return Conversion.ToBytes((uint)data);
                }

                break;
            case 4051133705u:
                if (name == "Single")
                {
                    return Conversion.ToBytes((float)data);
                }

                break;
            case 3969205087u:
                if (name == "Boolean")
                {
                    return Conversion.ToBytes((bool)data);
                }

                break;
            case 3777191964u:
                if (name == "SByte[]")
                {
                    return Conversion.ToBytes((short[])data);
                }

                break;
        }

        throw new PrException(ExceptionCode.DataToWriteInvalid);
    }

    protected virtual ushort GetWriteDataLength(Units units, ushort numberOfDevicePoints)
    {
        return (ushort)((units == Units.Word) ? (numberOfDevicePoints * 2) : GetWriteBitDataLength(numberOfDevicePoints));
    }

    protected virtual ushort GetWriteBitDataLength(ushort NumberOfDevicePoints)
    {
        if (NumberOfDevicePoints % 2 == 0)
        {
            return (ushort)(NumberOfDevicePoints / 2);
        }

        return (ushort)(NumberOfDevicePoints / 2 + 1);
    }
}
}