using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CS.Service.Monitor
{
    // Utility static class
    public static class Utils
    {
        // Converts unix time stamp to DateTime
        public static DateTime UnixTimeStampToDateTimeS(long unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            if (unixTimeStamp < 0) return dtDateTime;
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }        

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            if (unixTimeStamp < 0) return dtDateTime;
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        public static double GetUnixTime(DateTime date)
        {
            var t = date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return t.TotalMilliseconds;
        }

        public static DateTime DtSec(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);
        }

        
        // Gets block age string by given TimeSpan
     

        // Converts binary hash into HEX string
        public static string ConvertHash(byte[] hash)
        {
            var hex = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        // Converts HEX string to binary hash
        public static byte[] ConvertHashBack(string hash)
        {
            var bytes = new List<byte>();
            for (var i = 0; i < hash.Length / 2; i++)
                bytes.Add(Convert.ToByte(hash.Substring(i * 2, 2), 16));
            return bytes.ToArray();
        }

        // Converts binary hash to ASCII string
        public static string ConvertHashAscii(byte[] hash)
        {
            return Encoding.ASCII.GetString(hash);
        }

        // Converts ASCII string to binary hash
        public static byte[] ConvertHashBackAscii(string hash)
        {
            return Encoding.ASCII.GetBytes(hash);
        }

        // Converts ASCII hash to Base58 hash
        public static string ConvertHashPartial(string hash)
        {
            return SimpleBase.Base58.Bitcoin.Encode(ConvertHashBack(hash));
        }

        // Converts Base58 hash to ASCII hash
        public static string ConvertHashBackPartial(string hash)
        {
            return ConvertHash(SimpleBase.Base58.Bitcoin.Decode(hash).ToArray());
        }

        /// <summary>
        /// Formats java source code from one line of code to multiline string
        /// </summary>
        /// <param name="code">Source code</param>
        /// <param name="lineNumbers">Need to insert line numbers</param>
        /// <returns></returns>
        public static string FormatSrc(string code, bool lineNumbers = false)
        {
            // If code is already multi-line, just return it
            if (code.Contains(Environment.NewLine) || code.Contains("\n")) return code;

            var sb = new StringBuilder();
            const int ident = 4;
            int level = 0, line = 1;
            var newLine = false;
            if(lineNumbers) sb.Append($"{line++:D3}| ".Replace('0', ' '));
            foreach (var c in code)
            {
                if (c == '{')
                {
                    if (lineNumbers) sb.AppendLine();
                    if (lineNumbers) sb.Append(' ', level * ident);
                    sb.Append(c);
                    level++;
                    newLine = true;
                }
                else if (c == '}')
                {
                    level--;
                    sb.AppendLine();
                    if (lineNumbers) sb.Append($"{line++:D3}| ".Replace('0', ' '));
                    sb.Append(' ', level * ident);
                    sb.Append(c);
                    newLine = true;
                }
                else if (c == ';')
                {
                    sb.Append(c);
                    newLine = true;
                }
                else if (c == ' ' && newLine)
                {
                }
                else
                {
                    if (newLine)
                    {
                        sb.AppendLine();
                        if (lineNumbers) sb.Append($"{line++:D3}| ".Replace('0', ' '));
                        sb.Append(' ', level * ident);
                        newLine = false;
                    }
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Calculates total number of pages
        /// </summary>
        /// <param name="count">Total count of items</param>
        /// <param name="numPerPage">Number of items displayed at one page</param>
        /// <returns></returns>
        public static int GetNumPages(long count, int numPerPage)
        {
            if (count <= 0) return 1;
            if (count % numPerPage == 0) return (int) count / numPerPage;
            return (int) count / numPerPage + 1;
        }

        /// <summary>
        /// Partially hides IP address
        /// </summary>
        /// <param name="ip">Input IP address</param>
        /// <returns>Partially hidden IP address</returns>
        public static string GetIpCut(string ip)
        {
            if (!ip.Contains(":"))
            {
                // Ipv4
                var split = ip.Split('.');
                if (split.Length != 4) return ip;
                return string.Join('.', split.Take(2)) + ".*.*";
                //return string.Join('.', split.Take(2)) + $".{new string('*', split[2].Length)}.{new string('*', split[3].Length)}";
            }
            else
            {
                // Ipv6
                var split = ip.Split(':');
                var take = split.Length > 2 ? split.Length - 2 : split.Length;
                return string.Join(':', split.Take(take)) + ":*:*";
            }
        }

        public static string SimplifyJavaType(string type)
        {
            return type.Replace("java.lang.", "", StringComparison.InvariantCultureIgnoreCase)
                        .Replace("java.math.", "", StringComparison.InvariantCultureIgnoreCase);
        }

        public static string ConvertCommission(short c, bool roundFee = false)
        {
            var sign = c >> 15;
            var m = c & 0x3FF;
            var f = (c >> 10) & 0x1F;
            const double v1024 = 1.0 / 1024;
            var num = (sign != 0u ? -1.0 : 1.0) * m * v1024 * Math.Pow(10.0, f - 18);
            if (roundFee) num = Math.Round(num, 5);
            return num.ToString(CultureInfo.InvariantCulture);
        }

        public static byte[] ReadFully(this Stream input)
        {
            var ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
