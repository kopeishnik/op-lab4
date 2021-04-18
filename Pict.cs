using System;
using System.IO;

namespace bmpTest
{
    public class Pict
    {
        private Byte[] first2bites;
        private Int32 _flSize;
        private Byte[] next8bites;
        private Int32 bcSize;
        private Int16 _pictW16;
        private Int16 _pictH16;
        private Int32 _pictW32;
        private Int32 _pictH32;
        private byte[] lastBites;
        private byte[][][] _data;

        public Pict(string path)
        {
            BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open));
            first2bites = br.ReadBytes(2);
            _flSize = br.ReadInt32();
            next8bites = br.ReadBytes(8);
            bcSize = br.ReadInt32();
            if (bcSize == 12)
            {
                _pictW16 = br.ReadInt16();
                _pictH16 = br.ReadInt16();
                _pictW32 = _pictH32 = 0;
                lastBites = br.ReadBytes(4);
            }
            else if (bcSize == 40)
            {
                _pictW32 = br.ReadInt32();
                _pictH32 = br.ReadInt32();
                _pictW16 = _pictH16 = 0;
                lastBites = br.ReadBytes(28);
            }
            else
            {
                br.Close();
                    Console.WriteLine("can't read(");
                    Console.WriteLine(bcSize);
                    Environment.Exit(0);
            }
            _data = new byte[(_pictH16>_pictH32?_pictH16:_pictH32)][][];
            for (int i = 0; i < (_pictH16>_pictH32?_pictH16:_pictH32); i++)
            {
                _data[i] = new byte[(_pictW16>_pictW32?_pictW16:_pictW32)][];
                for (int j = 0; j < (_pictW16>_pictW32?_pictW16:_pictW32); j++)
                {
                    _data[i][j] = br.ReadBytes(3);
                }

                if (((_pictW16>_pictW32?_pictW16:_pictW32) * 3) % 4 > 0) br.ReadBytes(4 - (((_pictW16>_pictW32?_pictW16:_pictW32) * 3) % 4));
            }
            br.Close();
        }
        public void ToFile(string path)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create));
            bw.Write(first2bites);
            bw.Write(_flSize);
            bw.Write(next8bites);
            bw.Write(bcSize);
            bw.Write((_pictW16>_pictW32?_pictW16:_pictW32));
            bw.Write((_pictH16>_pictH32?_pictH16:_pictH32));
            bw.Write(lastBites);
            for (int i = 0; i < (_pictH16>_pictH32?_pictH16:_pictH32); i++)
            {
                for (int j = 0; j < (_pictW16>_pictW32?_pictW16:_pictW32); j++)
                {
                    bw.Write(_data[i][j]);
                }

                if (((_pictW16>_pictW32?_pictW16:_pictW32) * 3) % 4 > 0) for (int ctr = 0; ctr < 4 - (((_pictW16>_pictW32?_pictW16:_pictW32) * 3) % 4); ctr++) bw.Write(false);
            }
        }
        public void Enlarge(Int16 coef)
        {
            byte[][][] nData = new byte[(_pictH16>_pictH32?_pictH16:_pictH32)*coef][][];
            for (int i = 0; i < (_pictH16 > _pictH32 ? _pictH16 : _pictH32); i++)
            {
                for (int ctr = 0; ctr < coef; ctr++)
                {
                    nData[i*coef + ctr] = new byte[(_pictW16 > _pictW32 ? _pictW16 : _pictW32) * coef][];
                    for (int j = 0; j < (_pictW16 > _pictW32 ? _pictW16 : _pictW32); j++)
                    {
                        for (int ctr2 = 0; ctr2 < coef; ctr2++)
                        {
                            nData[i * coef + ctr][j * coef + ctr] = _data[i][j];
                        }
                    }
                }
            }
            _data = nData;
            _pictH16 *= coef;
            _pictH32 *= coef;
            _pictW16 *= coef;
            _pictW32 *= coef;
            _flSize = 14 + bcSize + (_pictH16 > _pictH32
                ? (_pictW16 * 3 % 4 == 0 ? _pictW16 * _pictH16 * 3 : (_pictW16 * 3 + 4 - _pictW16 * 3 % 4) * _pictH16)
                : (_pictW32 * 3 % 4 == 0 ? _pictW32 * _pictH32 * 3 : (_pictW32 * 3 + 4 - _pictW32 * 3 % 4) * _pictH32));
        }
    }
}
