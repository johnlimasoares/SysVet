using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils {
    public static class RemoverEspeciais {
        public static string ApenasNumeros(this string parametro)
        {
            return Regex.Replace(parametro, "[^0-9]", string.Empty);
        }

    }
}

