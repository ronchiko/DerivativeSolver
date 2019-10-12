using System;
using System.Text;

namespace DerivativeCalculator
{
    public struct CleanerNode
    {
        private Identifier[] identifiers;
        private float value;

        public CleanerNode(float value, Identifier[] identifiers)
        {
            this.value = value;
            this.identifiers = identifiers;
        }

        public static bool CanAdd(CleanerNode a, CleanerNode b){
            return Identifier.CanAdd(a.identifiers, b.identifiers);
        }

        public static CleanerNode operator *(CleanerNode a, CleanerNode b)
        {
            return new CleanerNode(a.value * b.value, Identifier.Multiply(a.identifiers,b.identifiers));
        }
        public static CleanerNode operator /(CleanerNode a, CleanerNode b)
        {
            return new CleanerNode(a.value / b.value, Identifier.Divide(a.identifiers, b.identifiers));
        }

        public static CleanerNode operator +(CleanerNode a, CleanerNode b)
        {
            if (!CanAdd(a, b)) throw new Exception(string.Format("Cannot add {0} and {1}.", a.ToString(), b.ToString()));
            return new CleanerNode(a.value + b.value, a.identifiers);
        }
        public static CleanerNode operator -(CleanerNode a, CleanerNode b)
        {
            if (!CanAdd(a, b)) throw new Exception(string.Format("Cannot add {0} and {1}.", a.ToString(), b.ToString()));

            return new CleanerNode(a.value - b.value, a.identifiers);
        }

        public override string ToString()
        {
            if (value == 0) return "0";

            StringBuilder sb = new StringBuilder();
            if(value != 1) sb.Append(value);
            for (int i = 0; i < identifiers.Length; i++)
            {
                sb.Append(identifiers[i]);
            }
            return sb.ToString();
        }
    }
}
