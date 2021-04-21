using System;
using System.IO;

namespace bmpTest
{
    public class Picture
    {
        public Byte[] first2bites;
        public Int32 _flSize;
        public Byte[] next8bites;
        public Int32 bcSize;
        public Int16 _pictW16;
        public Int16 _pictH16;
        public Int32 _pictW32;
        public Int32 _pictH32;
        public int _realH;
        public int _realW;
        public byte[] lastBites;
        public byte[][][] _data;

        public Picture()
        {

        }
    }
}
