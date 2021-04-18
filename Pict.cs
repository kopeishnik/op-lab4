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
        private Int16 pictW;
        private Int16 pictH;
        private byte[] next4bites;
        private byte[][][] data;

        public Pict(string path)
        {
            BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open));
            first2bites = br.ReadBytes(2);
            flSize = br.ReadInt32();
            next8bites = br.ReadBytes(8);
            bcSize = br.ReadInt32();
            if (bcSize != 12)
            {
                br.Close();
                Console.WriteLine("can't read(");
                Console.WriteLine(bcSize);
                Environment.Exit(0);
            }
            pictW = br.ReadInt16();
            pictH = br.ReadInt16();
            next4bites = br.ReadBytes(4);
            data = new byte[pictH][][];
            for (int i = 0; i < pictH; i++)
            {
                data[i] = new byte[pictW][];
                for (int j = 0; j < pictW; j++)
                {
                    data[i][j] = br.ReadBytes(3);
                }

                if ((pictW * 3) % 4 > 0) br.ReadBytes(4 - ((pictW * 3) % 4));
            }
        }
        public void ToFile(string path)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create));
            bw.Write(first2bites);
            bw.Write(flSize);
            bw.Write(next8bites);
            bw.Write(bcSize);
            bw.Write(pictW);
            bw.Write(pictH);
            bw.Write(next4bites);
            for (int i = 0; i < pictH; i++)
            {
                for (int j = 0; j < pictW; j++)
                {
                    bw.Write(data[i][j]);
                }

                if ((pictW * 3) % 4 > 0) for (int ctr = 0; ctr < 4 - ((pictW * 3) % 4); ctr++) bw.Write(false);
            }
        }
        /*public Pict Enlarge(int coef)
        {
            
        }*/
    }
}
