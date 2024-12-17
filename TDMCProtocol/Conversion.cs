using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMCProtocol
{
    internal static class Conversion
    {
        public static byte ToByte(byte[] bytes)
        {
            if (bytes.Length != 1)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            return bytes[0];
        }

        public static byte[] ToByteArray(byte[] bytes)
        {
            if (bytes.Length == 0)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            return bytes;
        }

        public static sbyte ToSByte(byte[] bytes)
        {
            if (bytes.Length != 1)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            return (sbyte)bytes[0];
        }

        public static sbyte[] ToSByteArray(byte[] bytes)
        {
            if (bytes.Length == 0 && bytes.Length != 1)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            sbyte[] array = new sbyte[bytes.Length];
            for (short num = 0; num < bytes.Length; num++)
            {
                array[num] = (sbyte)bytes[num];
            }

            return array;
        }

        public static ushort ToUInt16(byte[] bytes)
        {
            if (bytes.Length != 2)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            return BitConverter.ToUInt16(bytes, 0);
        }

        public static ushort[] ToUInt16Array(byte[] bytes)
        {
            if ((bytes.Length % 2 != 0) & (bytes.Length < 2))
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            ushort[] array = new ushort[bytes.Length / 2];
            int num = 0;
            for (int i = 0; i < bytes.Length; i += 2)
            {
                array[num] = BitConverter.ToUInt16(bytes, i);
                num++;
            }

            return array;
        }

        public static short ToInt16(byte[] bytes)
        {
            if (bytes.Length != 2)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            return BitConverter.ToInt16(bytes, 0);
        }

        public static short[] ToInt16Array(byte[] bytes)
        {
            if ((bytes.Length % 2 != 0) & (bytes.Length < 2))
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            short[] array = new short[bytes.Length / 2];
            int num = 0;
            for (int i = 0; i < bytes.Length; i += 2)
            {
                array[num] = BitConverter.ToInt16(bytes, i);
                num++;
            }

            return array;
        }

        public static uint ToUInt32(byte[] bytes)
        {
            if (bytes.Length != 4)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            return BitConverter.ToUInt32(bytes, 0);
        }

        public static uint[] ToUInt32Array(byte[] bytes)
        {
            if ((bytes.Length % 4 != 0) & (bytes.Length < 4))
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            uint[] array = new uint[bytes.Length / 4];
            int num = 0;
            for (int i = 0; i < bytes.Length; i += 4)
            {
                array[num] = BitConverter.ToUInt32(bytes, i);
                num++;
            }

            return array;
        }

        public static int ToInt32(byte[] bytes)
        {
            if (bytes.Length != 4)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            return BitConverter.ToInt32(bytes, 0);
        }

        public static int[] ToInt32Array(byte[] bytes)
        {
            if ((bytes.Length % 4 != 0) & (bytes.Length < 4))
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            int[] array = new int[bytes.Length / 4];
            int num = 0;
            for (int i = 0; i < bytes.Length; i += 4)
            {
                array[num] = BitConverter.ToInt32(bytes, i);
                num++;
            }

            return array;
        }

        public static float ToSingle(byte[] bytes)
        {
            if (bytes.Length != 4)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            return BitConverter.ToSingle(bytes, 0);
        }

        public static float[] ToSingleArray(byte[] bytes)
        {
            if ((bytes.Length % 4 != 0) & (bytes.Length < 4))
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            float[] array = new float[bytes.Length / 4];
            int num = 0;
            for (int i = 0; i < bytes.Length; i += 4)
            {
                array[num] = BitConverter.ToSingle(bytes, i);
                num++;
            }

            return array;
        }

        public static string ToString(byte[] bytes)
        {
            try
            {
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }
        }

        public static double ToDouble(byte[] bytes)
        {
            if (bytes.Length != 8)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            return BitConverter.ToDouble(bytes, 0);
        }

        public static double[] ToDoubleArray(byte[] bytes)
        {
            if ((bytes.Length % 8 != 0) & (bytes.Length < 8))
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            double[] array = new double[bytes.Length / 8];
            int num = 0;
            for (int i = 0; i < bytes.Length; i += 8)
            {
                array[num] = BitConverter.ToDouble(bytes, i);
                num++;
            }

            return array;
        }

        public static bool ToBoolean(byte[] bytes)
        {
            if (bytes.Length != 1)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            return GetBitInByte(bytes[0], 4);
        }

        public static bool[] ToBooleanArray(byte[] bytes)
        {
            if (bytes.Length < 1)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            bool[] array = new bool[bytes.Length * 2];
            ushort num = 0;
            foreach (byte aByte in bytes)
            {
                array[num] = GetBitInByte(aByte, 4);
                array[num + 1] = GetBitInByte(aByte, 0);
                num += 2;
            }

            return array;
        }

        public static bool[] ToBooleanArray(byte[] bytes, ushort count)
        {
            if (bytes.Length < 1)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            bool[] array = new bool[count];
            ushort num = 0;
            for (ushort num2 = 0; num2 < bytes.Length; num2++)
            {
                array[num] = GetBitInByte(bytes[num2], 4);
                if (num <= count - 2)
                {
                    array[num + 1] = GetBitInByte(bytes[num2], 0);
                }

                num += 2;
            }

            return array;
        }

        public static byte[] ToBytes(bool value)
        {
            return new byte[1] { SetBitInByte(0, value, 4) };
        }

        public static byte[] ToBytes(bool[] values)
        {
            if (values.Length == 0)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            if (values.Length == 1)
            {
                return new byte[1] { SetBitInByte(0, values[0], 4) };
            }

            if (values.Length > 1)
            {
                ByteArray byteArray = new ByteArray();
                bool flag = true;
                foreach (bool value in values)
                {
                    if (flag)
                    {
                        byteArray.Add(SetBitInByte(0, value, 4));
                        flag = false;
                    }
                    else
                    {
                        byteArray[byteArray.Lenght - 1] = SetBitInByte(byteArray[byteArray.Lenght - 1], value, 0);
                        flag = true;
                    }
                }

                return byteArray.array;
            }

            return null;
        }

        public static byte[] ToBytes(ushort value)
        {
            return BitConverter.GetBytes((short)value);
        }

        public static byte[] ToBytes(ushort[] values)
        {
            if (values.Length == 0)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            if (values.Length == 1)
            {
                return BitConverter.GetBytes((short)values[0]);
            }

            if (values.Length > 1)
            {
                ByteArray byteArray = new ByteArray();
                foreach (ushort num in values)
                {
                    byteArray.Add(BitConverter.GetBytes((short)num));
                }

                return byteArray.array;
            }

            return null;
        }

        public static byte[] ToBytes(short value)
        {
            return BitConverter.GetBytes(value);
        }

        public static byte[] ToBytes(short[] values)
        {
            if (values.Length == 0)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            if (values.Length == 1)
            {
                return BitConverter.GetBytes(values[0]);
            }

            if (values.Length > 1)
            {
                ByteArray byteArray = new ByteArray();
                foreach (short value in values)
                {
                    byteArray.Add(BitConverter.GetBytes(value));
                }

                return byteArray.array;
            }

            return null;
        }

        public static byte[] ToBytes(uint value)
        {
            return BitConverter.GetBytes((short)value);
        }

        public static byte[] ToBytes(uint[] values)
        {
            if (values.Length == 0)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            if (values.Length == 1)
            {
                return BitConverter.GetBytes((short)values[0]);
            }

            if (values.Length > 1)
            {
                ByteArray byteArray = new ByteArray();
                foreach (uint num in values)
                {
                    byteArray.Add(BitConverter.GetBytes((short)num));
                }

                return byteArray.array;
            }

            return null;
        }

        public static byte[] ToBytes(int value)
        {
            return BitConverter.GetBytes((short)value);
        }

        public static byte[] ToBytes(int[] values)
        {
            if (values.Length == 0)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            if (values.Length == 1)
            {
                return BitConverter.GetBytes((short)values[0]);
            }

            if (values.Length > 1)
            {
                ByteArray byteArray = new ByteArray();
                foreach (int num in values)
                {
                    byteArray.Add(BitConverter.GetBytes((short)num));
                }

                return byteArray.array;
            }

            return null;
        }

        public static byte[] ToBytes(float value)
        {
            return BitConverter.GetBytes((short)value);
        }

        public static byte[] ToBytes(float[] values)
        {
            if (values.Length == 0)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            if (values.Length == 1)
            {
                return BitConverter.GetBytes((short)values[0]);
            }

            if (values.Length > 1)
            {
                ByteArray byteArray = new ByteArray();
                foreach (float num in values)
                {
                    byteArray.Add(BitConverter.GetBytes((short)num));
                }

                return byteArray.array;
            }

            return null;
        }

        public static byte[] ToBytes(double value)
        {
            return BitConverter.GetBytes((short)value);
        }

        public static byte[] ToBytes(double[] values)
        {
            if (values.Length == 0)
            {
                throw new PrException(ExceptionCode.DataConversionFault);
            }

            if (values.Length == 1)
            {
                return BitConverter.GetBytes((short)values[0]);
            }

            if (values.Length > 1)
            {
                ByteArray byteArray = new ByteArray();
                foreach (double num in values)
                {
                    byteArray.Add(BitConverter.GetBytes((short)num));
                }

                return byteArray.array;
            }

            return null;
        }

        public static byte[] ToBytes(string values)
        {
            ByteArray byteArray = new ByteArray();
            for (int i = 0; i < values.Length; i++)
            {
                byte item = (byte)values[i];
                byteArray.Add(item);
            }

            if (byteArray.Lenght % 2 != 0)
            {
                byteArray.Add(0);
            }

            return byteArray.array;
        }

        private static bool GetBitInByte(byte aByte, byte pos)
        {
            return (aByte & (1 << (int)pos)) != 0;
        }

        private static byte SetBitInByte(byte aByte, bool value, byte pos)
        {
            byte b = (byte)(1 << (int)pos);
            if (value)
            {
                return aByte |= b;
            }

            return aByte &= (byte)(~b);
        }
    }
}
