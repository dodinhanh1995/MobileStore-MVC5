using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public static class Convertion
    {
        public static string ToUnsignString(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace("_", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2.Replace("--", "-").ToLower();
        }

        public static string WordRepresentByText(string text)
        {
            var arr = text.Split(' ');
            return arr[arr.Length - 1].Substring(0, 1).ToUpper();
        }

        public static string ConvertDateTimeToString(DateTime dt)
        {
            TimeSpan now = (DateTime.Now - dt);
            StringBuilder output = new StringBuilder();
            if (now.TotalSeconds < 60)
                output.AppendFormat("{0} giây trước", Math.Floor(now.TotalSeconds));
            else if (now.TotalMinutes < 60)
                output.AppendFormat("{0} phút trước", Math.Floor(now.TotalMinutes));
            else if (now.TotalHours < 24)
                output.AppendFormat("{0} giờ trước", Math.Floor(now.TotalHours));
            else if (now.TotalDays < 7)
                output.AppendFormat("{0} ngày trước", Math.Floor(now.TotalDays));
            else if (now.TotalDays < 30)
                output.AppendFormat("{0} tuần trước", Math.Floor(now.TotalDays / 7));
            else
                output.AppendFormat(dt.ToShortDateString());
            return output.ToString();
        }

        public static string Truncate(string content, int number)
        {
            int length = content.Length;
            if (length > number)
            {
                content = content.Substring(0, number);
                if (number + 3 > length)
                    for (int i = 0; i < length - number; i++)
                    {
                        content += ".";
                    }
                else
                    content += "...";
            }
            return content;
        }
    }
}
