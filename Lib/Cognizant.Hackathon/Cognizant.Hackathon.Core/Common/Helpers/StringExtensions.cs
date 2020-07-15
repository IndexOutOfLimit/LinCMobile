using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cognizant.Hackathon.Core.Common.Helpers
{
    public static class StringExtensions
    {
        #region Delegates

        /// <summary>
        /// The parse delegate.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        private delegate bool ParseDelegate<T>(string s, out T result);

        #endregion

        #region Public Methods and Operators

        public static bool IsNumber(this string text)
        {
            return double.TryParse(text, out var number);
        }

        /// <summary>
        /// The append path.
        /// </summary>
        /// <param name="basepath">
        /// The basepath.
        /// </param>
        /// <param name="relativepath">
        /// The relativepath.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AppendPath(this string basepath, string relativepath)
        {
            if (basepath == null || relativepath == null)
            {
                return null;
            }

            var chars = new char[2];
            chars[0] = '/';
            chars[1] = '\\';

            if (basepath.Contains("\\"))
            {
                return Path.Combine(basepath.TrimEnd(chars), relativepath.TrimStart(chars)).Replace('/', '\\');
            }

            return Path.Combine(basepath.TrimEnd(chars), relativepath.TrimStart(chars)).Replace('\\', '/');
        }

        /// <summary>
        /// The clean.
        /// </summary>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Clean(this string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : str.ToLower();
        }

        /// <summary>
        /// The to title case.
        /// </summary>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToTitleCase(this string str)
        {
            return Regex.Replace(
                str,
                @"\w+",
                (m) =>
                {
                    string tmp = m.Value;
                    return char.ToUpper(tmp[0]) + tmp.Substring(1, tmp.Length - 1).ToLower();
                });
        }

        /// <summary>
        /// The try parse boolean.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryParseBoolean(this string value)
        {
            return TryParse<bool>(value, bool.TryParse);
        }

        /// <summary>
        /// The try parse boolean.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool TryParseBoolean(this string value, bool defaultValue)
        {
            return TryParse(value, bool.TryParse, defaultValue);
        }

        /// <summary>
        /// The try parse byte.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="byte"/>.
        /// </returns>
        public static byte TryParseByte(this string value)
        {
            return TryParse<byte>(value, byte.TryParse);
        }

        /// <summary>
        /// The try parse byte.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="byte"/>.
        /// </returns>
        public static byte TryParseByte(this string value, byte defaultValue)
        {
            return TryParse(value, byte.TryParse, defaultValue);
        }

        /// <summary>
        /// The try parse date time.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime TryParseDateTime(this string value)
        {
            return TryParse<DateTime>(value, DateTime.TryParse);
        }

        /// <summary>
        /// The try parse date time.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime TryParseDateTime(this string value, DateTime defaultValue)
        {
            return TryParse(value, DateTime.TryParse, defaultValue);
        }

        /// <summary>
        /// The try parse date time utc.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime TryParseDateTimeUtc(this string value)
        {
            return TryParse<DateTime>(value, DateTime.TryParse).ToUniversalTime();
        }

        /// <summary>
        /// The try parse date time utc.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime TryParseDateTimeUtc(this string value, DateTime defaultValue)
        {
            return TryParse(value, DateTime.TryParse, defaultValue).ToUniversalTime();
        }

        /// <summary>
        /// The try parse decimal.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="decimal"/>.
        /// </returns>
        public static decimal TryParseDecimal(this string value)
        {
            return TryParse<decimal>(value, decimal.TryParse);
        }

        /// <summary>
        /// The try parse decimal.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="decimal"/>.
        /// </returns>
        public static decimal TryParseDecimal(this string value, decimal defaultValue)
        {
            return TryParse(value, decimal.TryParse, defaultValue);
        }

        /// <summary>
        /// The try parse double.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        public static double TryParseDouble(this string value)
        {
            return TryParse<double>(value, double.TryParse);
        }

        /// <summary>
        /// The try parse double.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        public static double TryParseDouble(this string value, double defaultValue)
        {
            return TryParse(value, double.TryParse, defaultValue);
        }

        /// <summary>
        /// The try parse int 16.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="short"/>.
        /// </returns>
        public static short TryParseInt16(this string value)
        {
            return TryParse<short>(value, short.TryParse);
        }

        /// <summary>
        /// The try parse int 16.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="short"/>.
        /// </returns>
        public static short TryParseInt16(this string value, short defaultValue)
        {
            return TryParse(value, short.TryParse, defaultValue);
        }

        /// <summary>
        /// The try parse int 32.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int TryParseInt32(this string value)
        {
            return TryParse<int>(value, int.TryParse);
        }

        /// <summary>
        /// The try parse int 32.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int TryParseInt32(this string value, int defaultValue)
        {
            return TryParse(value, int.TryParse, defaultValue);
        }

        /// <summary>
        /// The try parse int 64.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        public static long TryParseInt64(this string value)
        {
            return TryParse<long>(value, long.TryParse);
        }

        /// <summary>
        /// The try parse int 64.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        public static long TryParseInt64(this string value, long defaultValue)
        {
            return TryParse(value, long.TryParse, defaultValue);
        }

        /// <summary>
        /// The try parse guid.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/>.
        /// </returns>
        public static Guid TryParseGuid(this string value, Guid defaultValue)
        {
            var guid = TryParse(value, Guid.TryParse, defaultValue);
            return guid == Guid.Empty ? defaultValue : guid;
        }

        /// <summary>
        /// The try parse single.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        public static float TryParseSingle(this string value)
        {
            return TryParse<float>(value, float.TryParse);
        }

        /// <summary>
        /// The try parse single.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        public static float TryParseSingle(this string value, float defaultValue)
        {
            return TryParse(value, float.TryParse, defaultValue);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The try parse.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="parse">
        /// The parse.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        private static T TryParse<T>(this string value, ParseDelegate<T> parse) where T : struct
        {
            T result;
            parse(value, out result);
            return result;
        }

        /// <summary>
        /// The try parse.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="parse">
        /// The parse.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        private static T TryParse<T>(this string value, ParseDelegate<T> parse, T defaultValue) where T : struct
        {
            T result;

            // if ( string.IsNullOrEmpty( value ) )
            // return defaultValue;
            if (!parse(value, out result))
            {
                return defaultValue;
            }
            else
            {
                return result;
            }
        }

        #endregion

    }
}
