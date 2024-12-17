using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMCProtocol
{
    public class ByteArray
    {
        private List<byte> list = new List<byte>();

        public int Lenght => list.Count;

        public byte this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        public byte[] array => list.ToArray();

        public ByteArray()
        {
            list = new List<byte>();
        }

        public ByteArray(int size)
        {
            list = new List<byte>(size);
        }

        public void Clear()
        {
            list = new List<byte>();
        }

        public void Add(byte item)
        {
            list.Add(item);
        }

        public void Add(byte[] items)
        {
            list.AddRange(items);
        }

        public void Add(ByteArray byteArray)
        {
            list.AddRange(byteArray.array);
        }

        public void Remove(int index)
        {
            list.RemoveAt(index);
        }

        public void Remove()
        {
            if (list.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
