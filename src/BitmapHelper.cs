/***************************************************
*创建人:rixiang.yu
*创建时间:2017/6/15 15:24:10
*功能说明:<Function>
*版权所有:<Copyright>
*Frameworkversion:4.0
*CLR版本：4.0.30319.42000
***************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
namespace UcAsp.Gainscha
{
    public static class BitmapHelper
    {
        /// <summary>
        /// 转换单色位图的方法
        /// </summary>
        /// <param name="img"></param>
        private static Bitmap TowColor(Bitmap img)
        {
            int w = img.Width;
            int h = img.Height;
            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format1bppIndexed);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            for (int y = 0; y < h; y++)
            {
                byte[] scan = new byte[(w + 7) / 8];
                for (int x = 0; x < w; x++)
                {
                    Color c = img.GetPixel(x, y);
                    if (c.GetBrightness() >= 0.5) scan[x / 8] |= (byte)(0x80 >> (x % 8));
                }
                Marshal.Copy(scan, 0, (IntPtr)(data.Scan0 + data.Stride * y), scan.Length);
            }
            bmp.Palette = CreateGrayscalePalette();

            return bmp;

        }
        /// <summary>
        /// 创建单色灰度调色板
        /// </summary>
        /// <returns>返回调色板</returns>
        private static ColorPalette CreateGrayscalePalette()
        {
            ColorPalette palette = CreateColorPalette(PixelFormat.Format1bppIndexed);
            palette.Entries[0] = Color.FromArgb(0, 0, 0, 0);
            palette.Entries[1] = Color.FromArgb(0, 255, 255, 255);
            return palette;
        }
        /// 创建图像格式对应的调色板
        /// </summary>
        /// <param name="pixelFormat">图像格式，只能是Format1bppIndexed,Format1bppIndexed,Format1bppIndexed</param>
        /// <returns>返回调色板；如果创建失败或者图像格式不支持，返回null。</returns>
        private static ColorPalette CreateColorPalette(PixelFormat pixelFormat)
        {
            ColorPalette palette = null;
            if (pixelFormat == PixelFormat.Format1bppIndexed || pixelFormat == PixelFormat.Format4bppIndexed || pixelFormat == PixelFormat.Format8bppIndexed)
            {
                //因为ColorPalette类没有构造函数，所以这里创建一个1x1的位图，然后抓取该位图的调色板
                Bitmap temp = new Bitmap(1, 1, pixelFormat);
                palette = temp.Palette;
                temp.Dispose();
            }
            return palette;
        }

        /// <summary>
        /// 转换为单色位图后，图像信息头中的位图使用的颜色数及指定重要的颜色数设置为0
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ConvertToBpp(Bitmap bitMap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap bm = TowColor(bitMap);
                bm.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);

                byte[] bitLenght = new byte[4];
                Array.Copy(buffer, 2, bitLenght, 0, 4);
                int ibitlen = bytesToInt(bitLenght, 0);
                Array.Copy(bitLenght, 0, buffer, 34, 4);
                byte[] bitmapBuffer = new byte[1];
                bitmapBuffer[0] = 0;
                Array.Copy(bitmapBuffer, 0, buffer, 46, 1);
                Array.Copy(bitmapBuffer, 0, buffer, 50, 1);
                bm.Dispose();
                return buffer;
            }
        }
        /**
* byte数组中取int数值，本方法适用于(低位在前，高位在后)的顺序。
*
* @param ary
* byte数组
* @param offset
* 从数组的第offset位开始
* @return int数值
*/
        private static int bytesToInt(byte[] ary, int offset)
        {
            int value;
            value = (int)((ary[offset] & 0xFF)
            | ((ary[offset + 1] << 8) & 0xFF00)
            | ((ary[offset + 2] << 16) & 0xFF0000)
            | ((ary[offset + 3] << 24) & 0xFF000000));
            return value;
        }
    }
}
