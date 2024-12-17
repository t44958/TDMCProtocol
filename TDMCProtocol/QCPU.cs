using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Sockets;
namespace TDMCProtocol
{


 

    public class QCPU : PlcBase
    {
        public ushort MonitoringTimer { get; private set; }

        public QCPU(string ip, int port)
            : base(ip, port)
        {
            MonitoringTimer = 16;
        }

        public QCPU(string ip, int port, ushort monitoringTimer)
            : base(ip, port)
        {
            MonitoringTimer = monitoringTimer;
        }

        public override object Read(string variableName, VarType varType)
        {
            return Read(variableName, varType, 1);
        }

        public override object Read(string variableName, VarType varType, ushort varCount)
        {
            variableName = variableName.ToUpper();
            variableName = variableName.Replace(" ", "");
            try
            {
                return Read(GetDeviceName(variableName), GetDeviceNumber(variableName), varType, varCount);
            }
            catch (PrException ex)
            {
                base.LastErrorCode = (byte)ex.ExCode;
                OnProtocolException(this, ex.ExCode, variableName + ": " + varType);
                return GetDefault(varType, varCount);
            }
            catch
            {
                base.LastErrorCode = byte.MaxValue;
                OnProtocolException(this, ExceptionCode.Unknown, variableName + ": " + varType);
                return GetDefault(varType, varCount);
            }
        }

        protected override object Read(DeviceName deviceName, uint deviceNumber, VarType varType, ushort varCount)
        {
            try
            {
                Units units = ((varType == VarType.Bit) ? Units.Bit : Units.Word);
                ushort count = NumberOfDevicePoints(varType, varCount);
                byte[] bytes = ReadBytesWithASingleRequest(deviceName, deviceNumber, units, count);
                return ParseBytes(varType, bytes, varCount);
            }
            catch (PrException ex)
            {
                throw ex;
            }
        }

        protected override byte[] CreateReadDataRequestPackage(DeviceName deviceName, uint deviceNumber, Units units, ushort count = 1)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.Add(BitConverter.GetBytes((short)80));
            byteArray.Add(0);
            byteArray.Add(byte.MaxValue);
            byteArray.Add(BitConverter.GetBytes((short)1023));
            byteArray.Add(0);
            byteArray.Add(BitConverter.GetBytes((short)12));
            byteArray.Add(BitConverter.GetBytes((short)MonitoringTimer));
            byteArray.Add(CommandToBytes(Command.Read));
            byteArray.Add(SubCommadToBytes(units));
            byteArray.Add(DeviceNumberToBytes(deviceNumber));
            byteArray.Add(DeviceNameToBytes(deviceName));
            byteArray.Add(BitConverter.GetBytes((short)count));
            return byteArray.array;
        }

        protected override byte[] ReadBytesWithASingleRequest(DeviceName device, uint deviceNumber, Units units, ushort count)
        {
            lock (this)
            {
                try
                {
                    byte[] array = CreateReadDataRequestPackage(device, deviceNumber, units, count);
                    _mSocket.Send(array, array.Length, SocketFlags.None);
                    byte[] array2 = new byte[2048];
                    _mSocket.Receive(array2, SocketFlags.None);
                    if (BitConverter.ToInt16(array2, 9) != 0)
                    {
                        throw new PrException(ExceptionCode.ResponseAbnormalCompletion);
                    }

                    int num = BitConverter.ToInt32(array2, 7) - 2;
                    ByteArray byteArray = new ByteArray();
                    for (int i = 0; i < num; i++)
                    {
                        byteArray.Add(array2[i + 11]);
                    }

                    return byteArray.array;
                }
                catch (PrException ex)
                {
                    throw ex;
                }
                catch
                {
                    throw new PrException(ExceptionCode.ConnectionFailt);
                }
            }
        }

        protected override uint GetDeviceNumber(string variableName)
        {
            variableName = variableName.Replace(" ", "");
            variableName.ToUpper();
            try
            {
                switch (GetDeviceName(variableName))
                {
                    case DeviceName.InternalRelay:
                    return     Convert.ToUInt32(variableName.Substring(1), 10);
                       
                    case DeviceName.SpecialRelay:
                        return Convert.ToUInt32(variableName.Substring(2), 10);
                    case
                        DeviceName.LatchRelay: return Convert.ToUInt32(variableName.Substring(1), 10);
                    case DeviceName.Annunciator : return Convert.ToUInt32(variableName.Substring(1), 10);
                  case DeviceName.EdgeRelay: return Convert.ToUInt32(variableName.Substring(1), 10);
                  case DeviceName.Input : return Convert.ToUInt32(variableName.Substring(1), 16);
                  case DeviceName.Output : return Convert.ToUInt32(variableName.Substring(1), 16);
                  case DeviceName.LinkRelay: return Convert.ToUInt32(variableName.Substring(1), 16);
                  case DeviceName.LinkSpecialRelay : return Convert.ToUInt32(variableName.Substring(2), 16);
                  case DeviceName.DirectAccessInput : return Convert.ToUInt32(variableName.Substring(2), 16);
                 case DeviceName.DirectAccessOutput : return Convert.ToUInt32(variableName.Substring(2), 16);
                 case DeviceName.DataRegister : return Convert.ToUInt32(variableName.Substring(1), 10);
                  case DeviceName.SpecialRegister : return Convert.ToUInt32(variableName.Substring(2), 10);
                 case DeviceName.LinkRegister : return Convert.ToUInt32(variableName.Substring(1), 16);
                 case DeviceName.LinkSpecialRegister : return Convert.ToUInt32(variableName.Substring(2), 16);
                 case DeviceName.Timer : return Convert.ToUInt32(variableName.Substring(1), 10);
                case DeviceName.Counter : return Convert.ToUInt32(variableName.Substring(1), 10);
                 case DeviceName.STimer : return Convert.ToUInt32(variableName.Substring(2), 10);
                 case DeviceName.IndexRegister : return Convert.ToUInt32(variableName.Substring(1), 10);
                }
                return 0;
                //return  GetDeviceName(variableName) switch
                //{
                //    DeviceName.InternalRelay => Convert.ToUInt32(variableName.Substring(1), 10),
                //    DeviceName.SpecialRelay => Convert.ToUInt32(variableName.Substring(2), 10),
                //    DeviceName.LatchRelay => Convert.ToUInt32(variableName.Substring(1), 10),
                //    DeviceName.Annunciator => Convert.ToUInt32(variableName.Substring(1), 10),
                //    DeviceName.EdgeRelay => Convert.ToUInt32(variableName.Substring(1), 10),
                //    DeviceName.Input => Convert.ToUInt32(variableName.Substring(1), 16),
                //    DeviceName.Output => Convert.ToUInt32(variableName.Substring(1), 16),
                //    DeviceName.LinkRelay => Convert.ToUInt32(variableName.Substring(1), 16),
                //    DeviceName.LinkSpecialRelay => Convert.ToUInt32(variableName.Substring(2), 16),
                //    DeviceName.DirectAccessInput => Convert.ToUInt32(variableName.Substring(2), 16),
                //    DeviceName.DirectAccessOutput => Convert.ToUInt32(variableName.Substring(2), 16),
                //    DeviceName.DataRegister => Convert.ToUInt32(variableName.Substring(1), 10),
                //    DeviceName.SpecialRegister => Convert.ToUInt32(variableName.Substring(2), 10),
                //    DeviceName.LinkRegister => Convert.ToUInt32(variableName.Substring(1), 16),
                //    DeviceName.LinkSpecialRegister => Convert.ToUInt32(variableName.Substring(2), 16),
                //    DeviceName.Timer => Convert.ToUInt32(variableName.Substring(1), 10),
                //    DeviceName.Counter => Convert.ToUInt32(variableName.Substring(1), 10),
                //    DeviceName.STimer => Convert.ToUInt32(variableName.Substring(2), 10),
                //    DeviceName.IndexRegister => Convert.ToUInt32(variableName.Substring(1), 10),
                //    _ => throw new PrException(ExceptionCode.WrongVariableFormat),
                //};
            }
            catch
            {
                throw new PrException(ExceptionCode.WrongVariableFormat);
            }
        }

        public override void Write(string variableName, VarType varType, object dataToWrite)
        {
            Write(variableName, varType, 1, dataToWrite);
        }

        public override void Write(string variableName, VarType varType, ushort varCount, object dataToWrite)
        {
            variableName = variableName.ToUpper();
            variableName = variableName.Replace(" ", "");
            try
            {
                Write(GetDeviceName(variableName), GetDeviceNumber(variableName), varType, varCount, dataToWrite);
            }
            catch (PrException ex)
            {
                base.LastErrorCode = (byte)ex.ExCode;
                OnProtocolException(this, ex.ExCode, variableName + ": " + varType);
            }
            catch
            {
                base.LastErrorCode = byte.MaxValue;
                OnProtocolException(this, ExceptionCode.Unknown, variableName + ": " + varType);
            }
        }

        protected override void Write(DeviceName deviceName, uint deviceNumber, VarType varType, ushort varCount, object dataToWrite)
        {
            try
            {
                Units units = ((varType == VarType.Bit) ? Units.Bit : Units.Word);
                ushort num = NumberOfDevicePoints(varType, varCount);
                byte[] data = ParseWriteData(GetDataToWrite(dataToWrite), units, num);
                WriteBytesWithASingleRequest(deviceName, deviceNumber, units, num, data);
            }
            catch (PrException ex)
            {
                throw ex;
            }
        }

        protected override byte[] CreateWriteDataRequestPackage(DeviceName deviceName, uint deviceNumber, Units units, ushort count, byte[] data)
        {
            ByteArray byteArray = new ByteArray();
            byteArray.Add(BitConverter.GetBytes((short)80));
            byteArray.Add(0);
            byteArray.Add(byte.MaxValue);
            byteArray.Add(BitConverter.GetBytes((short)1023));
            byteArray.Add(0);
            byteArray.Add(BitConverter.GetBytes((short)(12 + GetWriteDataLength(units, count))));
            byteArray.Add(BitConverter.GetBytes((short)MonitoringTimer));
            byteArray.Add(CommandToBytes(Command.Write));
            byteArray.Add(SubCommadToBytes(units));
            byteArray.Add(DeviceNumberToBytes(deviceNumber));
            byteArray.Add(DeviceNameToBytes(deviceName));
            byteArray.Add(BitConverter.GetBytes((short)count));
            byteArray.Add(data);
            return byteArray.array;
        }

        protected override void WriteBytesWithASingleRequest(DeviceName device, uint deviceNumber, Units units, ushort count, byte[] data)
        {
            lock (this)
            {
                try
                {
                    byte[] array = CreateWriteDataRequestPackage(device, deviceNumber, units, count, data);
                    _mSocket.Send(array, array.Length, SocketFlags.None);
                    byte[] array2 = new byte[128];
                    _mSocket.Receive(array2, SocketFlags.None);
                    if (BitConverter.ToInt16(array2, 9) != 0)
                    {
                        throw new PrException(ExceptionCode.ResponseAbnormalCompletion);
                    }
                }
                catch (PrException ex)
                {
                    throw ex;
                }
                catch
                {
                    throw new PrException(ExceptionCode.ConnectionFailt);
                }
            }
        }

        protected override byte[] DeviceNameToBytes(DeviceName deviceName)
        {
            return new byte[1] { (byte)deviceName };
        }

        protected override byte[] DeviceNumberToBytes(uint deviceNumber)
        {
            byte[] bytes = BitConverter.GetBytes((int)deviceNumber);
            ByteArray byteArray = new ByteArray();
            byteArray.Add(bytes);
            byteArray.Remove(3);
            return byteArray.array;
        }

        protected override byte[] SubCommadToBytes(Units units)
        {
            if (units != 0)
            {
                return BitConverter.GetBytes((short)1);
            }

            return BitConverter.GetBytes((short)0);
        }
    }
}
