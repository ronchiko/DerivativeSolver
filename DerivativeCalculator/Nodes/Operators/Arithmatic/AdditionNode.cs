namespace DerivativeCalculator
{
    internal class AdditionNode : OperatorNode
    {
        public AdditionNode() : base("+") { }

        public override string GetDerivative()
        {
            return Left.GetDerivative() + "+" + Right.GetDerivative(); 
        }

        public override string GetValue()
        {
            return Right.GetValue() + "+" + Left.GetValue();
        }
    }
}
