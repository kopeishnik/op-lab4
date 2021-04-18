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
        private static int szCoef = 1;
        static void Main()
        {
            // GetFromUser();
            _path = Environment.CurrentDirectory;
            _flName = @"\bmp.bmp";
            _nFlName = @"\new.bmp";
            //
            Pict inp = new Pict(_path + _flName);
            inp.ToFile(_path + _nFlName);
        }

        static void GetFromUser()
        {
            Console.Write("Enter the filename: ");
            _path = Environment.CurrentDirectory;
            _flName = Console.ReadLine();
            if (_flName.Length==0) _flName = @"\bmp.bmp";
            Console.Write("Enter the size coefficient: ");
            int n = Int32.Parse(Console.ReadLine());
            if (n > 0) szCoef = n;
        }
    }
}