namespace Unily.Member.Export.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

public static class StringExtensions
    {
        public static bool ParseStringToBoolean(this string target)
        {
            string[] booleanStringFalse = new string[] { "0", "off", "no", "n", "false" };
            string[] booleanStringTrue = new string[] { "1", "on", "yes", "y", "true" };

            if (string.IsNullOrEmpty(target))
            {
                return false;
            }
            else if (booleanStringFalse.Contains(target, StringComparer.InvariantCultureIgnoreCase))
            {
                return false;
            }
            else if (booleanStringTrue.Contains(target, StringComparer.InvariantCultureIgnoreCase))
            {
                return true;
            }

            bool result;
            bool.TryParse(target, out result);

            if (result)
                return true;
            else
                return false;
        }
        
        public static string NullIfEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }

        private static readonly List<(string LeftBracket, string RightBracket)> jsonBrackets = new List<(string, string)>
        {
            // Object
            ("{", "}"),
            // Array
            ("[", "]")
        };

        public static bool CouldBeJson(this string value)
        {
            return jsonBrackets.Any(x => value.StartsWith(x.LeftBracket) && value.EndsWith(x.RightBracket));
        }

        public static T DeserializeJson<T>(this string @string)
        {
            var type = typeof(T);
            if (type.IsClass && !@string.CouldBeJson())
                throw new ArgumentException($"String is not recognised as either a JSON object or array: {@string}", nameof(@string));

            return JsonConvert.DeserializeObject<T>(@string);
        }

        public static string[] SplitAndTrim(this string @string, char separator, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            return @string
                .Split(new[] { separator }, stringSplitOptions)
                .Select(x => x.Trim())
                .ToArray();
        }

        public static byte[] ConvertFingerprintToByteArray(this string @string)
        {
            return @string.Split(':').Select(s => Convert.ToByte(s, 16)).ToArray();
        }

        public static string ToBase64String(this string encode, Encoding encoding)
        {
            if (encode == null)
                return null;

            var encodeAsBytes = encoding.GetBytes(encode);
            return Convert.ToBase64String(encodeAsBytes);
        }
    }