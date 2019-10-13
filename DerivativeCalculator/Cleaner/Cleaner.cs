using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DerivativeCalculator
{
    public static class Cleaner
    {
        private static char[] Splitters = new char[]
        {
            '(', ')', '+', '-', '/', '*', '^'
        };

        
        private static byte GetOV(ICleanerNode node)
        {
            if (node.GetType() == typeof(CleanerOperatorNode))
                return GetOV(((CleanerOperatorNode)node).type);
            return 255;
        }

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
            Queue<ICleanerNode> q = Shant(exp);
            Stack<ICleanerNode> s = new Stack<ICleanerNode>();

            while (q.Count > 0)
            {
                ICleanerNode node = q.Dequeue();
                if(node.GetType() == typeof(CleanerOperatorNode))
                {
                    CleanerOperatorNode operatorNode = (CleanerOperatorNode)node;

                    IComputableCleanerNode n1 = s.Count > 0 ? (IComputableCleanerNode)s.Pop() : null;
                    IComputableCleanerNode n2 = s.Count > 0 ? (IComputableCleanerNode)s.Pop() : null;

                    ICleanerNode n = operatorNode.Execute(n2, n1);
                    s.Push(n);
                }
                else{
                    s.Push(node);
                }
            }

            return s.Pop().ToString();
        }

        private static Queue<ICleanerNode> Shant(string exp)
        {
            Queue<ICleanerNode> q = new Queue<ICleanerNode>();
            Stack<ICleanerNode> s = new Stack<ICleanerNode>();

            CleanerOperatorNode checker = new CleanerOperatorNode('(');
            StringBuilder sb = new StringBuilder();
            for(int i = 0;i < exp.Length; i++)
            {
                if (Splitters.Contains(exp[i]))
                {
                    if(sb.Length > 0)
                        q.Enqueue(CreateNode(sb.ToString()));
                    switch (exp[i])
                    {
                        case '(':
                            s.Push(new CleanerOperatorNode('('));
                            break;
                        case ')':
                            
                            while(s.Count > 0 && !checker.IsEqual(s.Peek()))
                            {
                                q.Enqueue(s.Pop());
                            }
                            if(s.Count > 0) s.Pop();
                            break;
                        default:
                            byte ov = GetOV(exp[i]);
                            while (s.Count > 0 && !checker.IsEqual(s.Peek()) && GetOV(s.Peek()) >= ov)
                            {
                                q.Enqueue(s.Pop());
                            }
                            s.Push(new CleanerOperatorNode(exp[i]));
                            break;
                    }
                    sb.Clear();
                    continue;
                }
                sb.Append(exp[i]);
            }
            if (sb.Length != 0)
                q.Enqueue(CreateNode(sb.ToString()));

            while (s.Count > 0)
            {
                q.Enqueue(s.Pop());
            }

            return q;
        }

        private static ICleanerNode CreateNode(string s)
        {
            int p;
            if(int.TryParse(s, out p))
            {
                return new CleanerNode(p, new Identifier[0]);
            }
            else
            {
                return new CleanerNode(1, new Identifier[1] { new Identifier(s, 1) });
            }
        }
    }
}
