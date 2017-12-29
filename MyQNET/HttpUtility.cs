using System;
using System.Collections.Generic;
using System.Text;

namespace MyQNET
{
    public sealed class HttpUtility
    {
        #region Constructors

        public HttpUtility()
        {
        }

        #endregion // Constructors

        sealed class HttpQSCollection : Dictionary<string, string>
        {
            public override string ToString()
            {
                int count = Count;
                if (count == 0)
                    return "";
                var sb = new StringBuilder();
                var keys = this.Keys;
                foreach (var key in this.Keys)
                {
                    sb.AppendFormat("{0}={1}&", key, this[key]);
                }
                if (sb.Length > 0)
                    sb.Length--;
                return sb.ToString();
            }
        }

        #region Methods

        public static Dictionary<string, string> ParseQueryString(string query)
        {
            return ParseQueryString(query, Encoding.UTF8);
        }

        public static Dictionary<string, string> ParseQueryString(string query, Encoding encoding)
        {
            if (query == null)
                throw new ArgumentNullException("query");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (query.Length == 0 || (query.Length == 1 && query[0] == '?'))
                return new HttpQSCollection();
            if (query[0] == '?')
                query = query.Substring(1);

            var result = new HttpQSCollection();
            ParseQueryString(query, encoding, result);
            return result;
        }

        internal static void ParseQueryString(string query, Encoding encoding, Dictionary<string, string> result)
        {
            if (query.Length == 0)
                return;

            var decoded = System.Net.WebUtility.HtmlDecode(query);
            var decodedLength = decoded.Length;
            var namePos = 0;
            var first = true;
            while (namePos <= decodedLength)
            {
                int valuePos = -1, valueEnd = -1;
                for (var q = namePos; q < decodedLength; q++)
                {
                    if (valuePos == -1 && decoded[q] == '=')
                    {
                        valuePos = q + 1;
                    }
                    else if (decoded[q] == '&')
                    {
                        valueEnd = q;
                        break;
                    }
                }

                if (first)
                {
                    first = false;
                    if (decoded[namePos] == '?')
                        namePos++;
                }

                string name, value;
                if (valuePos == -1)
                {
                    name = null;
                    valuePos = namePos;
                }
                else
                {
                    name = System.Net.WebUtility.UrlDecode(decoded.Substring(namePos, valuePos - namePos - 1));
                }
                if (valueEnd < 0)
                {
                    namePos = -1;
                    valueEnd = decoded.Length;
                }
                else
                {
                    namePos = valueEnd + 1;
                }
                value = System.Net.WebUtility.UrlDecode(decoded.Substring(valuePos, valueEnd - valuePos));

                result.Add(name, value);
                if (namePos == -1)
                    break;
            }
        }

        #endregion // Methods
    }
}