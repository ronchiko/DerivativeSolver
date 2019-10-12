using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeCalculator
{
    public static class Cleaner
    {
        private static byte GetOV(char op)
        {
            switch (op)
            {
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                    return 2;
                case '^':
                    return 3;
            }
            return 255;
        }

        public static string Clean(string exp)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }
    }
}
