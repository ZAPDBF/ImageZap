using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageZap
{

    [StructLayout(LayoutKind.Explicit)]
    public struct Int32Converter
    {
        [FieldOffset(0)] private int Value;
        [FieldOffset(0)] private byte Byte1;
        [FieldOffset(1)] private byte Byte2;
        [FieldOffset(2)] private byte Byte3;
        [FieldOffset(3)] private byte Byte4;

        public Int32Converter(int value)
        {
            Byte1 = Byte2 = Byte3 = Byte4 = 0;
            Value = value;
        }

        public static implicit operator Int32(Int32Converter value)
        {
            return value.Value;
        }

        public static implicit operator Int32Converter(int value)
        {
            return new Int32Converter(value);
        }
        public Byte[] ByteArray
        {
            get
            {


                Byte[] ba = { Byte1, Byte2, Byte3, Byte4 };

                return ba;




            }
        }

    }

   
}
