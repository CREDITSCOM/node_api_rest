using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using NodeApi;

namespace CS.Service.RestApiNode
{
    // Utility static class
    public static class Utils
    {
        private static readonly Dictionary<int, string> _feeDictionary = new Dictionary<int, string>()
        {
            { 0, "0.00" },
            { 17279, "0.008740234375" },
            { 17766, "0.0349609375" },
            { 18575, "0.1396484375" },
            { 20663, "17.87109375" },
            { 21397, "89.55078125" },
            { 21871, "358.3984375" },
            { 22675, "1435.546875" },
            { 23115, "5732.421875" },
            { 23787, "22949.21875" },
            { 24491, "91699.21875" },
            { 24952, "367187.50" },
            { 25750, "1464843.75" },
            { 26201, "5869140.625" },
            { 26864, "23437500.00" },
            { 18303, "0.08740234375" },
            { 18611, "0.1748046875" },
            { 18880, "0.4375" }
        };

        public static string FeeByIndex(int index)
        {
            return !_feeDictionary.TryGetValue(index, out var result) ? "0.00" : result;
        }

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

        // Gets block age by given time stamp
        //public static string GetAge(long time)
        //{
        //    //return UnixTimeStampToDateTime(time).ToString("dd.MM.yyyy hh:mm:ss.fff");
        //    if (time == 0) return "0";
        //    var span = DateTime.Now - UnixTimeStampToDateTime(time);
        //    return AgeStr(span);
        //}

        // Gets block age string by given TimeSpan
        //public static string AgeStr(TimeSpan span)
        //{
        //    if (!Settings.AllowNegativeTime && span < TimeSpan.Zero) span = TimeSpan.Zero;
        //    var res = span.Days != 0 ? span.Days + "d " : "";
        //    res += span.Hours != 0 || span.Days != 0 ? span.Hours + "h " : "";
        //    res += span.Minutes + "m " + span.Seconds + "s";
        //    return res;
        //}

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

        public static string VarToStr(Variant v)
        {
            var type = Type(v);
            switch (type)
            {
                case "boolean_box": return v.V_boolean_box.ToString();
                case "boolean": return v.V_boolean.ToString();
                case "byte": return v.V_byte.ToString();
                case "short": return v.V_short.ToString();
                case "int": return v.V_int.ToString();
                case "long": return v.V_long.ToString();
                case "float": return v.V_float.ToString(CultureInfo.InvariantCulture);
                case "double": return v.V_double.ToString(CultureInfo.InvariantCulture);
                case "string": return $"\"{v.V_string}\"";
                default: return "null";
            }
        }

        public static string Type(Variant v)
        {
            var s = v.ToString();
            var start = s.IndexOf('(') + 3;
            var end = s.IndexOf(':');
            if (end <= start) return null;
            var type = s.Substring(start, end - start);
            return type;
        }

        // Converts ASCII hash to Base58 hash
        //public static string ConvertHashPartial(string hash)
        //{
        //    return Base58Encoding.Encode(ConvertHashBack(hash));
        //}

        //// Converts Base58 hash to ASCII hash
        //public static string ConvertHashBackPartial(string hash)
        //{
        //    return ConvertHash(Base58Encoding.Decode(hash));
        //}

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
            if (lineNumbers) sb.Append($"{line++:D3}| ".Replace('0', ' '));
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
            if (count % numPerPage == 0) return (int)count / numPerPage;
            return (int)count / numPerPage + 1;
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
