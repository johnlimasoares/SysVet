using System;
using System.Text.RegularExpressions;

namespace Utils {
    public static class StringUtils {
        public static string ApenasNumeros(this string value) {
            if (string.IsNullOrEmpty(value)) {
                return string.Empty;
            }
            return Regex.Replace(value, "[^0-9]", string.Empty);
        }

        public static string RemoverCaracteresEspeciais(this string value) {
            const string pattern = @"(?i)[^0-9a-záéíóúàèìòùâêîôûãõç\s]";
            return Regex.Replace(value, pattern, string.Empty);
        }

        public static string Max(this string value, int max) {
            if (value == null) {
                return null;
            }

            if (value == string.Empty) {
                return string.Empty;
            }

            if (value.Length > max && max > 0) {
                return value.Substring(0, max);
            }

            return value;
        }

        public static int ToInteger(this string value) {
            value = ApenasNumeros(value);

            if (string.IsNullOrEmpty(value)) {
                return 0;
            }

            return Convert.ToInt32(value);
        }

    }
}

