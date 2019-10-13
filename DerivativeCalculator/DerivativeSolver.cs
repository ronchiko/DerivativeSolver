using System.Text;

namespace DerivativeCalculator
{
    public class DerivativeSolver
    {
        //1st derivative functions

        /// <summary>
        /// Returns the raw derivative of a function
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string GetDerivativeRaw(string func)
        {
            func = func.Replace(" ", "");
            func = FixFunctionExpression(func);

            INode root = CreateNode(func);

            return root.GetDerivative();
        }
        /// <summary>
        /// Returns the clean derivative of a function
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string GetDerivative(string func)
        {
            return Cleaner.Clean(GetDerivativeRaw(func));
        }
        

        //2nd derivative functions

        /// <summary>
        /// Returns the raw 2nd derivative of a function
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string Get2ndDerivativeRaw(string func)
        {
            return GetDerivativeRaw(GetDerivativeRaw(func));
        }
        /// <summary>
        /// Returns the clean 2nd derivative of a function
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string Get2ndDerivative(string func)
        {
            return Cleaner.Clean(Get2ndDerivativeRaw(func));
        }

        private static INode CreateNode(string func)
        {
            if (func.StartsWith("("))
            {
                int lvl = 1, i = 1;
                while (lvl > 0 && i < func.Length)
                {
                    if (func[i] == '(') lvl++;
                    if (func[i] == ')') lvl--;
                    i++;
                }
                if (i == func.Length)
                {
                    func = func.Substring(1, func.Length - 2);
                }
            }

            //Scan for most valueable operator
            int index = -1, level = 0, op = 0;
            for (int i = 0; i < func.Length; i++)
            {
                if (func[i] == '(') level++;
                else if (func[i] == ')') level--;

                if (level == 0)
                {
                    byte _op = GetOP(func[i].ToString());
                    if (_op != 0 && (_op < op || op == 0))
                    {
                        index = i;
                        op = _op;
                    }
                }
            }

            if(index == -1)
            {
                if (func == "x") return new BasicDerivativeNode(func);
                else return new NumberNode(func);
            }

            OperatorNode node = OperatorNode.CreateBaseNode(func[index]);

            node.Left = CreateNode(func.Substring(0, index));
            node.Right = CreateNode(func.Substring(index + 1));

            return node;
        }
        private static byte GetOP(string op)
        {
            switch (op)
            {
                case "+":
                case "-":
                    return 2;
                case "*":
                    return 3;
                case "/":
                    return 3;
                case "^":
                    return 4;
            }
            return 0;
        }
        

        /// <summary>
        /// Turns a human readable function expressio to a machine readable expression
        /// (Mostly by inserting * signs)
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string FixFunctionExpression(string func)
        {
            StringBuilder sb = new StringBuilder(func);

            int cv = 0;
            for (int i = 0; i < func.Length - 1; i++)
            {
                char current = func[i], next = func[i + 1];

                if (current == ')' && (IsLegalNumber(next) || next == 'x'))
                {
                    sb.Insert(i + 1 + cv, '*');
                    cv++;
                }
                else if (IsLegalNumber(current) && (next == 'x' || next == '(')) {
                    sb.Insert(i + 1 + cv, '*');
                    cv++;
                }else if(current == 'x' && next == '(')
                {
                    sb.Insert(i + 1 + cv, '*');
                    cv++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns true if the number is readable for a computer
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLegalNumber(char c)
        {
            return char.IsDigit(c) || c == 'e';
        }
    }
}
