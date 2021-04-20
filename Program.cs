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
            if (args.Length < 3)
            {
                Console.WriteLine("Too few arguments!");
                Console.Beep(); 
                return -1;
            }
            Pict inp = new Pict(_path + args[0]);
            _nFlName = args[1];
            _szCoef = Double.Parse(args[2]);
            if (_szCoef < 0)
            {
                Console.WriteLine("Mirroring the image!");
                inp.Mirror();
                _szCoef *= -1;
            }
            if (_szCoef == 0) {
                Console.WriteLine("What?.. 0? How?");
            }
            else if (_szCoef == Math.Round(_szCoef, MidpointRounding.ToNegativeInfinity) && _szCoef > 1)
            {
                inp.Enlarge((short)_szCoef);
            }
            else
            {
                inp.BiLinearInterpolation(_szCoef);
            }
            inp.ToFile(_path + _nFlName);
            Console.WriteLine("Success!");
            return 0;
        }
    }
}
