using System;

namespace DerivativeCalculator
{
    public class CleanerOperatorNode : ICleanerNode
    {
        public static readonly Identifier[] NO_ID = new Identifier[0];

        public char type;

        public CleanerOperatorNode(char type)
        {
            this.type = type;
        }

        public bool IsEqual(ICleanerNode n)
        {
            if(typeof(CleanerOperatorNode) == n.GetType())
            {
                return ((CleanerOperatorNode)n).type == type;
            }
            return false;
        }

        public ICleanerNode Execute(IComputableCleanerNode a, IComputableCleanerNode b)
        {
            switch (type)
            {
                case '+':
                    if (a == null) return b;
                    return a.Add(b);
                case '-':
                    if (a == null) return b.Multiply(new CleanerNode(-1, NO_ID));
                    return a.Sub(b);
                case '*':
                    return a.Multiply(b);
                case '/':
                    return a.Divide(b);
                case '^':
                    return a.Power(b);
            }
            throw new Exception(string.Format("Unsupported operation {0}", type));
        }

        public override string ToString()
        {
            return type.ToString();
        }
    }
}
