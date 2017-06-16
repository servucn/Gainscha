/***************************************************
*创建人:rixiang.yu
*创建时间:2017/6/15 9:55:23
*功能说明:<Function>
*版权所有:<Copyright>
*Frameworkversion:4.0
*CLR版本：4.0.30319.42000
***************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
namespace UcAsp.Gainscha
{
    public class GPPrinterHelper
    {
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private ArrayList arryList = new ArrayList();
        private int dotToMM = 8;

        public GPPrinterHelper()
        {

        }
        public GPPrinterHelper withOpen(string ip, int port)
        {
            socket.Connect(new IPEndPoint(IPAddress.Parse(ip), 9100));
            return this;
        }

        /// <summary>
        /// 设置打印机DPI
        /// </summary>
        /// <param name="dpi">200,or 300</param>
        /// <returns></returns>
        public GPPrinterHelper withDPI(int dpi)
        {
            if (dpi == 200)
                this.dotToMM = 8;
            if (dpi == 300)
                this.dotToMM = 12;

            return this;
        }

        /// <summary>
        /// 获取打印机型号；
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            try
            {
                byte[] version = Encoding.UTF8.GetBytes("~!T\r\n");
                socket.Send(version);
                byte[] buffer = new byte[1024];
                int l = socket.Receive(buffer);
                socket.Close();
                return Encoding.UTF8.GetString(buffer).Substring(0, l);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public GPPrinterHelper withFeed(int x)
        {
            byte[] feed = Encoding.UTF8.GetBytes("GAP " + x * dotToMM + " \r\n");
            arryList.AddRange(feed);
            return this;
        }
        /// <summary>
        /// 设置打印纸张和间距，厘米
        /// </summary>
        /// <param name="width">100.00</param>
        /// <param name="heith">150.00</param>
        /// <param name="left">2.00</param>
        /// <param name="length">2.00</param>
        /// <returns></returns>
        public GPPrinterHelper withStep(double width, double heith, double left, double length)
        {

            byte[] size = Encoding.UTF8.GetBytes("SIZE " + width + " mm," + heith + " mm\r\n");
            arryList.AddRange(size);
            byte[] gap = Encoding.UTF8.GetBytes("GAP " + left + " mm," + length + " mm\r\n");
            arryList.AddRange(gap);
            return this;
        }

        /// <summary>
        /// 设置打印速度
        /// </summary>
        /// <param name="s">1~6</param>
        /// <returns></returns>
        public GPPrinterHelper withSpeep(int s)
        {
            byte[] speed = Encoding.UTF8.GetBytes("SPEED " + s + "\r\n");
            arryList.AddRange(speed);
            return this;
        }
        /// <summary>
        /// 设置打印浓度；
        /// </summary>
        /// <param name="d">1~15</param>
        /// <returns></returns>
        public GPPrinterHelper withDensityd(int d)
        {
            byte[] density = Encoding.UTF8.GetBytes("DENSITY " + d + "\r\n");
            arryList.AddRange(density);
            return this;
        }
        /// <summary>
        /// 设置打印方向
        /// </summary>
        /// <param name="d">0~1</param>
        /// <returns></returns>
        public GPPrinterHelper withDirection(int d)
        {
            byte[] density = Encoding.UTF8.GetBytes("DIRECTION " + d + "\r\n");
            arryList.AddRange(density);
            return this;
        }
        /// <summary>
        /// 功能：定义标签纸上的相對于原点的参考点座标 单位厘米
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public GPPrinterHelper withFeference(int x, int y)
        {
            //TODO:200dpi *8
            //300dpi *12
            byte[] density = Encoding.UTF8.GetBytes("REFERENCE " + x * dotToMM + "," + y * dotToMM + "\r\n");
            arryList.AddRange(density);
            return this;
        }
        /// <summary>
        /// 此指令可将标签纸向前推送至下一张标签纸的起点开始打印
        /// </summary>
        /// <returns></returns>
        public GPPrinterHelper withHome()
        {
            byte[] home = Encoding.UTF8.GetBytes("HOME \r\n");
            arryList.AddRange(home);
            return this;
        }
        /// <summary>
        /// 下载文件到
        /// </summary>
        /// <param name="fileName">本地地址.bmp</param>
        /// <param name="printName">打印名称</param>
        /// <returns></returns>
        public GPPrinterHelper withDownload(string fileName, string printName)
        {

            Bitmap map = (Bitmap)Image.FromFile(fileName);
            byte[] bmp = BitmapHelper.ConvertToBpp(map);
            byte[] download = Encoding.UTF8.GetBytes("DOWNLOAD \"" + printName + "\"," + bmp.Length + ",");
            arryList.AddRange(download);
            arryList.AddRange(bmp);
            arryList.AddRange(Encoding.UTF8.GetBytes("\r\n"));
            map.Dispose();
            return this;



        }

        /// <summary>
        /// 打印图片；
        /// </summary>
        /// <param name="printName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public GPPrinterHelper withBitmap(string printName, int x, int y)
        {
            byte[] putbmp = Encoding.UTF8.GetBytes("PUTBMP " + x * dotToMM + "," + y * dotToMM + ",\"" + printName + "\"\r\n");
            arryList.AddRange(putbmp);
            return this;
        }
        /// <summary>
        /// 条码
        /// </summary>
        /// <param name="printName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public GPPrinterHelper withBarcode(string str, int x, int y, bool showfont)
        {
            byte[] putbmp = Encoding.UTF8.GetBytes("BARCODE " + x + "," + y + ",\"39\",96," + Convert.ToInt32(showfont).ToString() + ",0,2,4,\"" + str + "\"\r\n");
            arryList.AddRange(putbmp);
            return this;
        }

        /// <summary>
        /// 审查二维码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public GPPrinterHelper withQrcode(string str, int x, int y, int width)
        {
            byte[] putbmp = Encoding.UTF8.GetBytes("QRCODE " + x * dotToMM + ", " + y * dotToMM + ", H, " + width + ", A, 0, \"" + str + "\"\r\n");
            arryList.AddRange(putbmp);
            return this;
        }
        /// <summary>
        /// 打印文字
        /// </summary>
        /// <param name="str"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fontsize">font 字型名称
        /// 1 8 x 12 英数字体
        /// 2 12 x 20 英数字体
        /// 3 16 x 24 英数字体
        /// 4 24 x 32 英数字体
        /// 5 32 x 48 英数字体
        /// 6 14 x 19 英数字体OCR-B 
        /// 7 14 x 25 英数字体OCR-A  
        /// 8 21 x 27 英数字体OCR-B  
        /// TST24.BF2 繁体中文24x24 字体(大五码) 
        /// TSS24.BF2 简体中文24x24 字体(GB码) 
        /// K 韩文 24x24 字体(KS码)</param>
        /// <returns></returns>
        public GPPrinterHelper withText(string str, int x, int y, string fontsize)
        {
            byte[] text = Encoding.UTF8.GetBytes("TEXT " + x * dotToMM + "," + y * dotToMM + ",\"" + fontsize + "\",0,1,1,\"" + str + "\"\r\n");
            arryList.AddRange(text);
            return this;
        }
        public GPPrinterHelper withTextCN(string str, int x, int y, int zoom)
        {
            byte[] text = Encoding.UTF8.GetBytes("TEXT " + x * dotToMM + "," + y * dotToMM + ",\"TSS24.BF2\",0," + zoom + "," + zoom + ",\"");

            arryList.AddRange(text);
            arryList.AddRange(CodeHelper.UTFTOGB(str));
            arryList.AddRange(Encoding.UTF8.GetBytes("\"\r\n"));
            return this;
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <returns></returns>
        public GPPrinterHelper withClear()
        {

            byte[] cls = Encoding.UTF8.GetBytes("CLS \r\n");
            arryList.AddRange(cls);
            return this;
        }

        public void Pinter()
        {
            byte[] print = Encoding.UTF8.GetBytes("PRINT 1,1 \r\n");
            arryList.AddRange(print);
            byte[] send = (byte[])arryList.ToArray(typeof(byte));
            socket.Send(send);
            socket.Close();
        }

    }
}
