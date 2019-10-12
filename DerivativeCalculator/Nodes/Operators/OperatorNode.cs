namespace DerivativeCalculator
{
    internal abstract class OperatorNode : INode
    {
        public string Value { get; set; }
        public INode[] Children { get; private set; }

        public INode Left
        {
            get => Children[0];
            set => Children[0] = value;
        }
        public INode Right
        {
            get => Children[1];
            set => Children[1] = value;
        }

        public OperatorNode(string value)
        {
            Value = value;
            Children = new INode[2];
        }

        public abstract string GetDerivative();
        public abstract string GetValue();
        public static OperatorNode CreateBaseNode(char op)
        {
            switch (op)
            {
                case '*': return new MultiplyNode();
                case '+': return new AdditionNode();
                case '^': return new PowerNode();
            }
            throw new System.Exception(string.Format("No known operator {0}", op));
        }
    }
}
