using System;
using System.Collections.Generic;

namespace Utils {
    public static class FormatUtils {
        public static string FormatFone(this string value) {

            if (string.IsNullOrEmpty(value)) {
                return string.Empty;
            }

            var tamanho = value.Length;
            switch (tamanho) {
                case 10:
                    return Convert.ToInt64(value).ToString("(00)0000-0000");
                    break;
                case 11:
                    return Convert.ToInt64(value).ToString("(00)00000-0000");
                    break;
                default:
                    return Convert.ToInt64(value).ToString("(00)0000-0000");
            }
        }
    }
}