using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bmpTest
{
    class Resizer
    {
        public static void Enlarge(Picture pic, Int16 coef)
        {
            byte[][][] nData = new byte[pic._realH * coef][][];
            for (int i = 0; i < pic._realW; i++)
            {
                for (int ctr = 0; ctr < coef; ctr++)
                {
                    nData[i * coef + ctr] = new byte[pic._realW * coef][];
                    for (int j = 0; j < (pic._pictW16 > pic._pictW32 ? pic._pictW16 : pic._pictW32); j++)
                    {
                        for (int ctr2 = 0; ctr2 < coef; ctr2++)
                        {
                            nData[i * coef + ctr][j * coef + ctr2] = pic._data[i][j];
                        }
                    }
                }
            }
            pic._data = nData;
            pic._pictH16 *= coef; pic._pictH32 *= coef; pic._realH *= coef;
            pic._pictW16 *= coef; pic._pictW32 *= coef; pic._realW *= coef;
            pic._flSize = 14 + pic.bcSize + (pic._realW % 4 == 0
                ? pic._realW * pic._realH * 3
                : (pic._realW * 3 + 4 - pic._realW * 3 % 4) * pic._realH);
        }
        public static void Mirror(Picture pic)
        {
            byte[][][] _ndata = new byte[pic._realH][][];
            for (int i = 0; i < pic._realH; i++)
            {
                _ndata[i] = new byte[pic._realW][];
                for (int j = 0; j < pic._realW; j++)
                {
                    _ndata[i][j] = pic._data[i][pic._realW - 1 - j];
                }
            }
            pic._data = _ndata;
        }
        public static void BiLinearInterpolation(Picture pic, Double coef)
        {
            var newH = (int)Math.Round(pic._realH * coef, MidpointRounding.AwayFromZero);
            var newW = (int)Math.Round(pic._realW * coef, MidpointRounding.AwayFromZero);
            byte[][][] nData = new byte[pic._realH][][];
            for (int i = 0; i < pic._realH; i++)
            {
                nData[i] = new byte[newW][];
                for (int j = 0; j < newW; j++)
                {
                    nData[i][j] = new byte[3];
                    double placeInOld = (double)(j + 1) * pic._realW / newW;
                    if (placeInOld < 1) nData[i][j] = pic._data[i][0];
                    else if (placeInOld > (double)pic._realW - 1) nData[i][j] = pic._data[i][pic._realW - 1];
                    else
                    {
                        var lower = (int)Math.Round(placeInOld, MidpointRounding.ToNegativeInfinity);
                        double coefLeft = placeInOld - lower;
                        for (int k = 0; k < 3; k++)
                        {
                            var clr = (double)pic._data[i][lower][k] * coefLeft + (double)pic._data[i][lower + 1][k] * (1 - coefLeft);
                            nData[i][j][k] = (byte)Math.Round(clr, MidpointRounding.ToNegativeInfinity);
                        }
                    }
                }
            }
            byte[][][] nnData = new byte[newH][][];
            for (int i = 0; i < newH; i++)
            {
                nnData[i] = new byte[newW][];
            }
            for (int i = 0; i < newW; i++)
            {
                for (int j = 0; j < newH; j++)
                {
                    nnData[j][i] = new byte[3];
                    double placeInOld = (double)(j + 1) * pic._realH / newH;
                    if (placeInOld < 1) nnData[j][i] = nData[0][i];
                    else if (placeInOld > (double)pic._realH - 1) nnData[j][i] = nData[pic._realH - 1][i];
                    else
                    {
                        var upper = (int)Math.Round(placeInOld, MidpointRounding.ToNegativeInfinity);
                        double coefUp = placeInOld - upper;
                        for (int k = 0; k < 3; k++)
                        {
                            var clr = (double)nData[upper][i][k] * coefUp + (double)nData[upper + 1][i][k] * (1 - coefUp);
                            nnData[j][i][k] = (byte)Math.Round(clr, MidpointRounding.ToNegativeInfinity);
                        }
                    }
                }
            }
            pic._realH = newH;
            pic._realW = newH;
            if (pic._pictH16 == 0 && pic._pictW16 == 0)
            {
                pic._pictH32 = newH;
                pic._pictW32 = newW;
            }
            else if (pic._pictH32 == 0 && pic._pictW32 == 0)
            {
                pic._pictH16 = (short)newH;
                pic._pictW16 = (short)newW;
            }
            else
            {
                Console.WriteLine("Error hapenned!");
            }
            pic._data = nnData;
            pic._flSize = 14 + pic.bcSize + (pic._realW % 4 == 0
                ? pic._realW * pic._realH * 3
                : (pic._realW * 3 + 4 - pic._realW * 3 % 4) * pic._realH);
        }
    }
}

