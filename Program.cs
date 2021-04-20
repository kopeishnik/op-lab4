using System;
using System.IO;
using System.Text;

namespace bmpTest
{
    class Program
    {
        private static string _path;
        private static string _flName;
        private static string _nFlName;
        private static double _szCoef = 1;

        static int Main(string[] args)
        {
            _path = Environment.CurrentDirectory+@"\";
            if (args.Length < 4)
            {
                Console.WriteLine("Too few arguments!");
                Console.Beep();
                return -1;
            }
            Pict inp = new Pict(_path + args[0]);
            _szCoef = Double.Parse(args[2]);
            _nFlName = args[3];
            if (_szCoef < 0)
            {
                Console.Beep(); // why not axaxaxax (pust budet)
                inp.Mirror();
                _szCoef *= -1;
            }
            switch (args[1])
            {
                case "--enlarge":
                    if (_szCoef>1&&_szCoef%1==0) inp.Enlarge((short)_szCoef);
                    else inp.Enlarge((short)_szCoef);
                    break;
                case "--reduce":
                    inp.Reduce((short)_szCoef); //workn't
                    break;
            }
            inp.ToFile(_path + _nFlName);
            Console.WriteLine("Success!");
            return 0;
        }
    }
}
