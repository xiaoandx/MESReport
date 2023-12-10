using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MR.Utility.Helper
{
    /// <summary>
    /// 通用工具封装
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// 生成版本号，用于CSS/JS版本发布
        /// </summary>
        /// <returns></returns>
        public static string CreateVersionNo()
        {
            string code = DateTime.Now.ToString("yyyyMMddHHmmss") + GenerateRandomNum(8);
            code = MD5Helper.MD5Encrypt32(code);
            return code;
        }

        /// <summary>
        /// 生成Excel表头字母
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string ConvertToTitle(int n)
        {
            if (n <= 26) return ((char)(n + 'A' - 1)).ToString();
            if (n % 26 == 0)
            {
                return ConvertToTitle(n / 26 - 1) + 'Z';
            }
            else
            {
                return ConvertToTitle(n / 26) + ConvertToTitle(n % 26);
            }
        }

        /// <summary>
        /// 自动生成编号  201008251145409865
        /// </summary>
        /// <returns></returns>
        public static string CreateNo()
        {
            Random random = new Random();
            string strRandom = random.Next(19, 99).ToString(); //生成编号 
            string code = DateTime.Now.ToString("yyyyMMddHHmmss") + strRandom;//形如
            return code;
        }

        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="codeNum">生成长度</param>
        /// <returns></returns>
        public static string RndNum(int codeNum)
        {
            StringBuilder sb = new StringBuilder(codeNum);
            Random rand = new Random();
            for (int i = 1; i < codeNum + 1; i++)
            {
                int t = rand.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();

        }

        //随机字母
        private static char[] arWords = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        //随机数字
        private static char[] arNumeric = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static char[] constant =
      {
        '0','1','2','3','4','5','6','7','8','9',
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
      };

        /// <summary>
        /// 获取随机字母
        /// </summary>
        /// <param name="iLength">所需生成随机字母的长度</param>
        /// <example>CommonHelper.GenerateRandomWords(4)，取得随机字母为：edge</example>
        /// <returns>返回随机字母</returns>
        public static string GenerateRandomWords(int iLength)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(26);
            Random rm = new Random();
            for (int i = 0; i < iLength; i++)
            {
                sb.Append(arWords[rm.Next(26)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 0~9数字中生成任意长度数字
        /// </summary>
        /// <param name="iLength"></param>
        /// <example>CommonHelper.GenerateRandomWords(4)，取得随机字母为：edge</example>
        /// <returns>返回随机数字</returns>
        public static string GenerateRandomNum(int iLength)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(10);
            Random rm = new Random();
            for (int i = 0; i < iLength; i++)
            {
                sb.Append(arNumeric[rm.Next(10)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成20位编号
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomNo()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + GenerateRandomNum(6);
        }

        /// <summary>
        /// 生成随机数字及字母组合
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateRandomWordsNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 处理显示时间，如：1天前，1小时前
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatTime(Object time)
        {
            string timeSpan = string.Empty;

            DateTime sendTime = Convert.ToDateTime(time);
            DateTime currentTime = DateTime.Now;

            TimeSpan span = currentTime.Subtract(sendTime);
            int day = span.Days;
            int hour = span.Hours;
            int minute = span.Minutes;
            int second = span.Seconds;

            if (day > 30)
            {
                timeSpan = sendTime.ToString("yyyy-MM-dd HH:mm");
            }
            else if (day > 0 && day <= 30)
            {
                timeSpan = day.ToString() + "天前";
            }
            else if (hour != 0)
            {
                timeSpan = hour.ToString() + "小时前";
            }
            else if (minute != 0)
            {
                timeSpan = minute.ToString() + "分钟前";
            }
            else
            {
                if (second == 0) second = 1;
                timeSpan = second.ToString() + "秒前";
            }

            return timeSpan;
        }


        /// <summary>
        ///  时间戳转本地时间-时间戳精确到秒
        /// </summary> 
        public static DateTime ToLocalTimeDateBySeconds(long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeSeconds(unix);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        ///  时间转时间戳Unix-时间戳精确到秒
        /// </summary> 
        public static long ToUnixTimestampBySeconds(DateTime dt)
        {
            DateTimeOffset dto = new DateTimeOffset(dt);
            return dto.ToUnixTimeSeconds();
        }


        /// <summary>
        ///  时间戳转本地时间-时间戳精确到毫秒
        /// </summary> 
        public static DateTime ToLocalTimeDateByMilliseconds(long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeMilliseconds(unix);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        ///  时间转时间戳Unix-时间戳精确到毫秒
        /// </summary> 
        public static long ToUnixTimestampByMilliseconds(DateTime dt)
        {
            DateTimeOffset dto = new DateTimeOffset(dt);
            return dto.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 过滤不安全字符，防止SQL注入
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSQLChar(string str)
        {
            if (str == String.Empty)
                return String.Empty;
            str = str.Replace("'", "");
            str = str.Replace(";", "");
            str = str.Replace(",", "");
            str = str.Replace("?", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("@", "");
            str = str.Replace("=", "");
            str = str.Replace("+", "");
            str = str.Replace("*", "");
            str = str.Replace("&", "");
            str = str.Replace("#", "");
            str = str.Replace("%", "");
            str = str.Replace("$", "");

            //删除与数据库相关的词
            str = Regex.Replace(str, "select", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "insert", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "delete from", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "count", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "drop table", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "truncate", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "asc", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "mid", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "char", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "xp_cmdshell", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "exec master", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net localgroup administrators", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "and", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net user", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "or", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "-", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "delete", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "drop", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "script", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "update", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "and", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "chr", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "master", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "truncate", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "declare", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "mid", "", RegexOptions.IgnoreCase);

            return str;
        }

        /// <summary>
        /// 过滤不安全字符，防止SQL注入
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSQLChar2(string str)
        {
            if (str == String.Empty)
                return String.Empty;
            str = str.Replace("'", "");

            //删除与数据库相关的词
            str = Regex.Replace(str, "select", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "insert", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "delete from", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "count", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "drop table", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "truncate", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "asc", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "mid", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "char", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "xp_cmdshell", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "exec master", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net localgroup administrators", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "and", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net user", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "or", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "-", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "delete", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "drop", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "script", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "update", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "and", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "chr", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "master", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "truncate", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "declare", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "mid", "", RegexOptions.IgnoreCase);

            return str;
        }

        /// <summary>
        /// 过滤溢出或SQL注入
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int ReplaceSQLInt(int num)
        {
            if (num > 9999999)
            {
                num = 9999999;
            }

            return num;
        }

        /// <summary>
        /// 格式化手机号显示
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string FormatPhone(string phone)
        {
            if (phone.Length == 11)
            {
                phone = phone.Substring(0, 3) + "****" + phone.Substring(7, 4);
            }

            return phone;
        }
    }
}
