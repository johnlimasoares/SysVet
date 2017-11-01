using System;

namespace Utils {
    public static class MathUtils {

        public static Double Round(this Double value, int decimals) {
            return Math.Round(value, decimals);
        }

        public static Double Round(this Double value) {
            return Math.Round(value, 2);
        }

        public static Decimal Round(this Decimal value, int decimals) {
            return Math.Round(value, decimals);
        }
        
        public static Decimal Round(this Decimal value) {
            return Math.Round(value, 2);
        }

        public static Decimal Round(this Decimal value, MidpointRounding midpoint) {
            return Math.Round(value, 2, midpoint);
        }

    }
}
