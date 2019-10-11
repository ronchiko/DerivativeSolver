namespace DerivativeCalculator
{
    public class DerivativeSolver
    {
        public static string GetDerivative(string func)
        {
            func = func.Replace(" ", "");

            //Generate Tree
            INode root = CreateNode(func);


            return root.GetDerivative();
        }

        private static INode CreateNode(string func)
        {
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
            }
            return 0;
        }

    }
}
