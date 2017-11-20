using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MobileStore.Helpers
{
    public static class StringHelper
    {
        public static IHtmlString ImageNameSigned(this HtmlHelper helper, string url)
        {
            Regex rgx = new Regex(@"(-|.jpg|.jpeg|.png|.gif)");
            url = url.Substring(url.LastIndexOf('/') + 1);
            return new HtmlString(rgx.Replace(url, " ").TrimEnd());
        }

        public static IHtmlString WordRepresentByText(this HtmlHelper helper, string text)
        {
            var arr = text.Split(' ');
            return new HtmlString(arr[arr.Length - 1].Substring(0, 1).ToUpper());
        }

        public static string ConvertDateTimeToString(this HtmlHelper helper, DateTime dt)
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

        
    }
}