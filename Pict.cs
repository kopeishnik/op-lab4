using System;
using System.IO;

namespace bmpTest
{
    public class Pict
    {
        private Byte[] first2bites;
        private Int32 flSize;
        private Byte[] next8bites;
        private Int32 bcSize;
        private Int16 pictW = 0;
        private Int16 pictH = 0;
        private Int32 pictW32 = 0;
        private Int32 pictH32 = 0;
        private byte[] lastBites;
        private byte[][][] data;

        public Pict(string path)
        {
            BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open));
            first2bites = br.ReadBytes(2);
            flSize = br.ReadInt32();
            next8bites = br.ReadBytes(8);
            bcSize = br.ReadInt32();
            if (bcSize == 12)
            {
                pictW = br.ReadInt16();
                pictH = br.ReadInt16();
                lastBites = br.ReadBytes(4);
            }
            else if (bcSize == 40)
            {
                pictW32 = br.ReadInt32();
                pictH32 = br.ReadInt32();
                lastBites = br.ReadBytes(28);
            }
            else
            {
                br.Close();
                    Console.WriteLine("can't read(");
                    Console.WriteLine(bcSize);
                    Environment.Exit(0);
            }
            data = new byte[(pictH>pictH32?pictH:pictH32)][][];
            for (int i = 0; i < (pictH>pictH32?pictH:pictH32); i++)
            {
                data[i] = new byte[(pictW>pictW32?pictW:pictW32)][];
                for (int j = 0; j < (pictW>pictW32?pictW:pictW32); j++)
                {
                    data[i][j] = br.ReadBytes(3);
                }

                if (((pictW>pictW32?pictW:pictW32) * 3) % 4 > 0) br.ReadBytes(4 - (((pictW>pictW32?pictW:pictW32) * 3) % 4));
            }
            br.Close();
        }
        public void ToFile(string path)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create));
            bw.Write(first2bites);
            bw.Write(flSize);
            bw.Write(next8bites);
            bw.Write(bcSize);
            bw.Write((pictW>pictW32?pictW:pictW32));
            bw.Write((pictH>pictH32?pictH:pictH32));
            bw.Write(lastBites);
            for (int i = 0; i < (pictH>pictH32?pictH:pictH32); i++)
            {
                for (int j = 0; j < (pictW>pictW32?pictW:pictW32); j++)
                {
                    bw.Write(data[i][j]);
                }

                if (((pictW>pictW32?pictW:pictW32) * 3) % 4 > 0) for (int ctr = 0; ctr < 4 - (((pictW>pictW32?pictW:pictW32) * 3) % 4); ctr++) bw.Write(false);
            }
        }
        public void Enlarge(Int16 coef)
        {
            byte[][][] nData = new byte[(pictH>pictH32?pictH:pictH32)*coef][][];
            for (int i = 0; i < (pictH > pictH32 ? pictH : pictH32); i++)
            {
                for (int ctr = 0; ctr < coef; ctr++)
                {
                    nData[i*coef + ctr] = new byte[(pictW > pictW32 ? pictW : pictW32) * coef][];
                    for (int j = 0; j < (pictW > pictW32 ? pictW : pictW32); j++)
                    {
                        for (int ctr2 = 0; ctr2 < coef; ctr2++)
                        {
                            nData[i * coef + ctr][j * coef + ctr] = data[i][j];
                        }
                    }
                }
            }
            data = nData;
            pictH *= coef;
            pictH32 *= coef;
            pictW *= coef;
            pictW32 *= coef;
            flSize = 14 + bcSize + (pictH > pictH32
                ? (pictW * 3 % 4 == 0 ? pictW * pictH * 3 : (pictW * 3 + 4 - pictW * 3 % 4) * pictH)
                : (pictW32 * 3 % 4 == 0 ? pictW32 * pictH32 * 3 : (pictW32 * 3 + 4 - pictW32 * 3 % 4) * pictH32));
        }
    }
}
