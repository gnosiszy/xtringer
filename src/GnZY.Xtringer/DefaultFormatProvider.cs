using System;
using System.Net;

namespace GnZY.Xtringer
{
    internal class DefaultFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            var result = arg.ToString();

            if (string.IsNullOrEmpty(format))
            {
                return result;
            }

            switch (format.ToUpper())
            {
                case "U": return result.ToUpper();
                case "L": return result.ToLower();
                case "UE": return Uri.EscapeDataString(result);
                case "UD": return Uri.UnescapeDataString(result);
                case "XE": return WebUtility.HtmlEncode(result);
                case "XD": return WebUtility.HtmlDecode(result);
                case "XAE": return WebUtility.HtmlEncode(Uri.EscapeDataString(result));
                case "XAD": return Uri.UnescapeDataString(WebUtility.HtmlDecode(result));
            }

            return string.Format("{0:" + format + "}", arg);
        }
    }
}
