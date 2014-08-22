using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GnZY.Xtringer
{
    public class XParser : DynamicObject
    {
        protected const string Expression = @"\{(?<name>[^\}\:]+)\:?(?<format>[^\}]+)?[\}]+";

        protected static readonly RegexOptions RegexOptions = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant;

        protected IDictionary<string, Func<object>> Values = new Dictionary<string, Func<object>>();

        public readonly IFormatProvider DefaultFormatter = new DefaultFormatProvider();

        public string this[string name]
        {
            get { return GetValue(name); }
            set { SetValue(name, () => value); }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Array.AsReadOnly(Values.Keys.ToArray());
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetValue(binder.Name);

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetValue(binder.Name, () => value);
            return true;
        }

        internal string GetValue(string name, string format = null, IFormatProvider formatter = null)
        {
            Func<object> getter;

            if (!Values.TryGetValue(name, out getter)) return string.Empty;

            if (format == null) return (getter() ?? string.Empty).ToString();

            if (formatter == null) formatter = DefaultFormatter;

            var customFormatter = formatter as ICustomFormatter;

            if (customFormatter != null)
            {
                return customFormatter.Format(format, getter(), formatter);
            }

            return string.Format("{0:" + format + "}", getter(), formatter);
        }

        internal void SetValue(string name, Func<object> getValue)
        {
            Values[name] = getValue;
        }

        public string Parse(string s, IFormatProvider formatter = null)
        {
            return Regex.Replace(s, Expression, item =>
            {
                var name = item.Groups["name"].Value;
                var format = item.Groups["format"].Value;
                var value = GetValue(name, format, formatter);

                if (Regex.IsMatch(value, Expression, RegexOptions))
                {
                    value = Parse(value, formatter);
                }

                return value;
            }, RegexOptions);
        }
    }
}
