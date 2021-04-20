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

        static string Main(string[] args)
        {
            _path = Environment.CurrentDirectory + @"\";
            if (args.Length < 4) return "Too few arguments!";
            Pict inp = new Pict(_path + args[0]);
            _szCoef = Double.Parse(args[2]);
            _nFlName = args[3];
            if (_szCoef < 0) return "Size coefficient should be positive!";
            switch (args[1])
            {
                case "--enlarge":
                    if (_szCoef > 1 && _szCoef % 1 == 0) inp.Enlarge((short)_szCoef);
                    else inp.Enlarge(_szCoef);
                    break;
                case "--reduce":
                    inp.Reduce(_szCoef);
                    break;
            }
            inp.ToFile(_path + _nFlName);
            return "Success!";
        }

        static void GetFromUser()
        {
            Console.Write("Enter the filename: ");
            _path = Environment.CurrentDirectory;
            _flName = Console.ReadLine();
            if (_flName==null||_flName.Length<5) _flName = @"\bmp.bmp";
            Console.Write("Enter the new filename: ");
            _nFlName = Console.ReadLine();
            if (_nFlName==null||_nFlName.Length<5) _nFlName = @"\new.bmp";
            Console.Write("Enter the size coefficient: ");
            string n = Console.ReadLine();
            if (n!=null&&Double.Parse(n)!=0) _szCoef = Double.Parse(n);
        }
    }
}
