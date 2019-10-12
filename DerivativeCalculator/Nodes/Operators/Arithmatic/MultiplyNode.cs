namespace DerivativeCalculator
{
    internal class MultiplyNode : OperatorNode
    {
        public MultiplyNode() : base("*") { }

        public override string GetDerivative()
        {
            if(Left.GetType() == typeof(NumberNode))
                return Left.GetValue() + "*" + Right.GetDerivative();
            if (Right.GetType() == typeof(NumberNode))
                return Left.GetDerivative() + "*" + Right.GetValue();
            return Left.GetDerivative() + "*" + Right.Value + "+" 
                + Left.Value + "*" + Right.GetDerivative();
        }

        public override string GetValue()
        {
            return Left.GetValue() + "*" + Right.GetValue();
        }
    }
}
