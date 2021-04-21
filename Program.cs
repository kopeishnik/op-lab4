using System;

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
            if (args.Length == 0) 
            {
                Console.WriteLine(_path);
                Console.WriteLine("Enter file name please:");
                _flName = Console.ReadLine();
                Console.WriteLine("Enter new file name:");
                _nFlName = Console.ReadLine();
                Console.WriteLine("How many times you want picture size?");
                _szCoef = double.Parse(Console.ReadLine());
            }
            else if (args.Length > 0 && args.Length < 3)
            {
                Console.WriteLine("Too few arguments!");
                Console.Beep(); 
                return -1;
            }
            else
            {
                _flName = args[0];
                _nFlName = args[1];
                _szCoef = Double.Parse(args[2]);
            }
            Picture inp = FileIO.ReadFile(_path + _flName);
            if (_szCoef < 0)
            {
                Console.WriteLine("Mirroring the image!");
                Resizer.Mirror(inp);
                _szCoef *= -1;
            }
            if (_szCoef == 0) {
                Console.WriteLine("What?.. 0? How?");
                return -1;
            }
            else if (_szCoef == Math.Round(_szCoef, MidpointRounding.ToNegativeInfinity) && _szCoef > 1)
            {
                Resizer.Enlarge(inp, (short)_szCoef);
            }
            else
            {
                Resizer.BiLinearInterpolation(inp, _szCoef);
            }
            //inp.ToFile(_path + _nFlName);
            FileIO.WriteFile(inp, _path + _nFlName);
            Console.WriteLine("Success!");
            return 0;
        }
    }
}
