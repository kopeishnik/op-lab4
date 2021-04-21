using System;
using System.IO;

namespace bmpTest
{
    class FileIO
    {
        public static Picture ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Input file does not exist");
                Environment.Exit(0);
            }
            Picture pic = new();
            BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open));
            pic.first2bites = br.ReadBytes(2);
            pic._flSize = br.ReadInt32();
            pic.next8bites = br.ReadBytes(8);
            pic.bcSize = br.ReadInt32();
            if (pic.bcSize == 12)
            {
                pic._realW = pic._pictW16 = br.ReadInt16();
                pic._realH = pic._pictH16 = br.ReadInt16();
                pic._pictW32 = pic._pictH32 = 0;
                pic.lastBites = br.ReadBytes(4);
            }
            else if (pic.bcSize == 40)
            {
                pic._realW = pic._pictW32 = br.ReadInt32();
                pic._realH = pic._pictH32 = br.ReadInt32();
                pic._pictW16 = pic._pictH16 = 0;
                pic.lastBites = br.ReadBytes(28);
            }
            else
            {
                br.Close();
                Console.WriteLine("can't read(");
                Console.WriteLine(pic.bcSize);
                Environment.Exit(0);
            }
            pic._data = new byte[pic._realH][][];
            for (int i = 0; i < pic._realH; i++)
            {
                pic._data[i] = new byte[pic._realW][];
                for (int j = 0; j < pic._realW; j++)
                {
                    pic._data[i][j] = br.ReadBytes(3);
                }
                int width = pic._realW * 3;
                while (width % 4 != 0)
                {
                    br.ReadByte();
                    width++;
                }
            }
            br.Close();
            return pic;
        }
        public static void WriteFile(Picture pct, string path)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                bw.Write(pct.first2bites);
                bw.Write(pct._flSize);
                bw.Write(pct.next8bites);
                bw.Write(pct.bcSize);
                bw.Write(pct._pictW16 > pct._pictW32 ? pct._pictW16 : pct._pictW32);
                bw.Write((pct._pictH16 > pct._pictH32 ? pct._pictH16 : pct._pictH32));
                bw.Write(pct.lastBites);
                for (int i = 0; i < pct._realH; i++)
                {
                    for (int j = 0; j < pct._realW; j++)
                    {
                        bw.Write(pct._data[i][j]);
                    }

                    int width = pct._realW * 3;
                    while (width % 4 != 0)
                    {
                        bw.Write((byte)0);
                        width++;
                    }
                }
            }
        }
    }
}
