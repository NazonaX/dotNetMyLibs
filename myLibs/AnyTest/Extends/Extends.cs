using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NazonaX.MyLibs.Extends
{
    public static class Extends
    {

        #region ConsoleExtends
        /// <summary>
        /// 获取下一个Int32(int)型;
        /// 如果遇到浮点数则向下强制转换为int型;
        /// 如果遇到字符则自动略过;
        /// </summary>
        /// <returns>返回一个int型值</returns>
        public static int ReadNextInt()
        {
            string str;
            while (true)
            {
                str = ReadNextUnit();
                if (str.IsNumberPattern())
                    break;
            }
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                x = (int)Convert.ToDouble(str);
            }
            return x;
        }

        /// <summary>
        /// 读取下一个double值;
        /// 读取过程中遇到其他不符合数字类型的字符串则丢弃
        /// </summary>
        /// <returns>返回一个double值</returns>
        public static double ReadNextDouble()
        {
            string str;
            while (true)
            {
                str = ReadNextUnit();
                if (str.IsNumberPattern())
                    break;
            }
            return Convert.ToDouble(str);
        }

        /// <summary>
        /// 读取下一个字符串类型单元;
        /// 单元以空白字符分割
        /// </summary>
        /// <returns>返回一个字符串单元</returns>
        public static string ReadNextStringUnit()
        {
            return ReadNextUnit();
        }

        /// <summary>
        /// 读取下一个长整型;
        /// 以空白字符分割;
        /// 未遇到数组类型则持续读取知道出现数字类型输入单元
        /// </summary>
        /// <returns>Int64</returns>
        public static long ReadNextLong()
        {
            string str;
            while (true)
            {
                str = ReadNextUnit();
                if (str.IsNumberPattern())
                    break;
            }
            return Convert.ToInt64(str);
        }

        /// <summary>
        /// 读取下一个短整形
        /// 以空白字符分割;
        /// 未遇到数组类型则持续读取知道出现数字类型输入单元
        /// </summary>
        /// <returns>Int16</returns>
        public static long ReadNextShort()
        {
            string str;
            while (true)
            {
                str = ReadNextUnit();
                if (str.IsNumberPattern())
                    break;
            }
            return Convert.ToInt16(str);
        }

        /// <summary>
        /// 读取下一个decimal;
        /// 以空白字符分割;
        /// 未遇到数组类型则持续读取知道出现数字类型输入单元
        /// </summary>
        /// <returns>decimal</returns>
        public static decimal ReadNextDecimal()
        {
            string str;
            while (true)
            {
                str = ReadNextUnit();
                if (str.IsNumberPattern())
                    break;
            }
            return Convert.ToDecimal(str);
        }

        /// <summary>
        /// 获取下一个单元字符串,以空白字符为分割界限
        /// </summary>
        /// <returns>返回一个string类型</returns>
        private static string ReadNextUnit()
        {
            char buffer;
            //bool next = false;
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                buffer = (char)Console.Read();
                if (!CheckForWhite(buffer))
                {
                    sb.Append(buffer);
                }
                else if (sb.Length > 0)
                {
                    break;
                }
            }
            return sb.ToString();
        }

        private static bool CheckForNumber(char buffer)
        {
            return (buffer >= '0' && buffer <= '9');
        }

        private static bool CheckForCharacter(char buffer)
        {
            return (buffer >= 'a' && buffer <= 'z'
                || buffer >= 'A' && buffer <= 'Z');
        }

        private static bool CheckForWhite(char buffer)
        {
            switch (buffer)
            {
                case '\u0009':
                case '\u000A':
                case '\u000B':
                case '\u000C':
                case '\u000D':
                case '\u0020':
                case '\u0085':
                case '\u00A0':
                case '\u1680':
                case '\u2000':
                case '\u2001':
                case '\u2002':
                case '\u2003':
                case '\u2004':
                case '\u2005':
                case '\u2006':
                case '\u2007':
                case '\u2008':
                case '\u2009':
                case '\u200A':
                case '\u2028':
                case '\u2029':
                case '\u202F':
                case '\u205F':
                case '\u3000':
                    return true;
                default:
                    return false;
            }
        }
        #endregion


        #region StringExtends
        /// <summary>
        /// 判断字符串是否符合数字模式
        /// </summary>
        /// <param name="str"></param>
        /// <returns>符合返回true，否则返回false</returns>
        public static bool IsNumberPattern(this string str)
        {
            String regexStr = @"^(([0-9]+)([.]?)([0-9]+))$"//int & double, like .12, 21. or 12.21
                + @"|^([.][0-9]+)$"
                + @"|^([0-9]+[.])$";
            //Console.WriteLine(Regex.IsMatch("123", regexStr));
            //Console.WriteLine(Regex.IsMatch("12d3", regexStr));
            //Console.WriteLine(Regex.IsMatch("d123", regexStr));
            //Console.WriteLine(Regex.IsMatch("123d", regexStr));
            //Console.WriteLine(Regex.IsMatch("d123d", regexStr));
            //Console.WriteLine("\n" + Regex.IsMatch("12.3", regexStr));
            //Console.WriteLine(Regex.IsMatch("a12.3", regexStr));
            //Console.WriteLine(Regex.IsMatch("123s12.3", regexStr));
            //Console.WriteLine(Regex.IsMatch("12.3dd", regexStr));
            //Console.WriteLine(Regex.IsMatch("12.d3", regexStr));
            //Console.WriteLine(Regex.IsMatch("12d.3", regexStr));
            //Console.WriteLine(Regex.IsMatch(".3", regexStr));
            //Console.WriteLine(Regex.IsMatch("12.", regexStr));
            return Regex.IsMatch(str, regexStr);
        }

        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns>int值或转换失败时null</returns>
        public static int? ToInt32(this string str)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 将字符串转换为Int16类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns>short值或转换失败时null</returns>
        public static Int16? ToInt16(this string str)
        {
            try
            {
                return Convert.ToInt16(str);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 将字符串转换为Int64类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns>long值或转换失败时null</returns>
        public static Int64? ToInt64(this string str)
        {
            try
            {
                return Convert.ToInt64(str);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 将字符串转换为double类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns>double值或转换失败时null</returns>
        public static double? ToDouble(this string str)
        {
            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 将字符串转换为decimal类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns>decimal值或转换失败时null</returns>
        public static decimal? ToDecimal(this string str)
        {
            try
            {
                return Convert.ToDecimal(str);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 通过java时间戳字符串转换为当前日期
        /// </summary>
        /// <param name="str"></param>
        /// <returns>DateTime或转换失败null</returns>
        public static DateTime? ToDateTimeFromJavaTimeStamp(this string str)
        {
            int hour = (DateTime.UtcNow.ToLocalTime() - DateTime.UtcNow).Hours;//表示该时区的时区补正
            DateTime dt_1970 = new DateTime(1970, 1, 1);
            long time = str.ToInt64() ?? 0;
            if (time == 0)
                return null;
            else
                return new DateTime(dt_1970.Ticks + time * 10000).AddHours(hour);
        }

        #endregion


        #region DateTimeExtends
        /// <summary>
        /// 将当前日期转换为java时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>长整型</returns>
        public static long ToJavaTimeStamp(this DateTime dt)
        {
            //考虑时区的获取
            int hour = DateTime.UtcNow.ToLocalTime().Hour  - DateTime.UtcNow.Hour;//表示该时区的时区补正
            DateTime dt2 = dt.AddHours(0 - hour);
            DateTime dt_1970 = new DateTime(1970, 1, 1);
            return (dt2.Ticks - dt_1970.Ticks) / 10000;
        }
        #endregion

        #region longExtends
        public static DateTime? ToDateTimeFromJavaTimeStamp(this long timestamp)
        {
            int hour = (DateTime.UtcNow.ToLocalTime() - DateTime.UtcNow).Hours;//表示该时区的时区补正
            DateTime dt_1970 = new DateTime(1970, 1, 1);
            try
            {
                return new DateTime(dt_1970.Ticks + timestamp * 10000).AddHours(hour);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }
        #endregion
    }
}
