using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Drawing.Imaging;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Image> imagesArray = new System.Collections.Generic.List<Image>();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"C:\CosaHelperLib");
            
            foreach (System.IO.FileInfo file in dir.GetFiles())
            {
                String ext = file.Extension;
                Console.WriteLine(file.FullName);
                if (ext.Equals(".ico"))
                {
                    Stream iconStream = new FileStream(file.FullName, FileMode.Open);
                    IconBitmapDecoder decoder = new IconBitmapDecoder(iconStream,BitmapCreateOptions.PreservePixelFormat,BitmapCacheOption.None);

                    foreach (var item in decoder.Frames)
                    {

                        var src = new System.Windows.Media.Imaging.FormatConvertedBitmap();
                        src.BeginInit();
                        src.Source = item;
                        src.EndInit();

                        Console.WriteLine(file.FullName + '_' + src.PixelHeight+'X'+src.PixelWidth);
                        //Console.WriteLine("Pixel Width: " + src.PixelWidth);
                        //Console.WriteLine("Pixel Height: " + src.PixelHeight);
                        //Console.WriteLine("Pixel Width: " + src.PixelWidth);
                        Console.WriteLine("Alpha Channel Threshold: " + src.AlphaThreshold);

                    }

                }
                Console.WriteLine();
            }
            Console.Read();
        }

        private static bool isAlphaBitMap(Bitmap bmp)
        {
            var bmpBounds = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData  bmpData = bmp.LockBits(bmpBounds, ImageLockMode.ReadOnly, bmp.PixelFormat);

            try
            {
                var rowDataLength = bmp.Width * 4;
                var buffer = new byte[rowDataLength];

                for (var y = 0; y < bmpData.Height; y++)
                {
                    Marshal.Copy((IntPtr)((int)bmpData.Scan0 + bmpData.Stride * y), buffer, 0, rowDataLength);

                    for (int p = 0; p < rowDataLength; p += 4)
                    {
                        if (buffer[p] > 0 && buffer[p] < 255)
                            return true;
                    }
                }


            }
            finally
            {
                bmp.UnlockBits(bmpData);
            }

            return false;
        }
    }
}
