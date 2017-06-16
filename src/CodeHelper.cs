/***************************************************
*创建人:rixiang.yu
*创建时间:2017/6/15 16:54:17
*功能说明:<Function>
*版权所有:<Copyright>
*Frameworkversion:4.0
*CLR版本：4.0.30319.42000
***************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UcAsp.Gainscha
{
    public class CodeHelper
    {
        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] UTFTOGB(string text)
        {
            Encoding GB = System.Text.Encoding.GetEncoding("gb2312");
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            buffer = System.Text.Encoding.Convert(Encoding.UTF8, GB, buffer);
            return buffer;
        }
    }
}
