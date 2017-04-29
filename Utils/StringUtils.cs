﻿using System;
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

       
    }
}

