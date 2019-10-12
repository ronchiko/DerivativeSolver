namespace DerivativeCalculator
{
    internal class PowerNode : OperatorNode
    {
        public PowerNode() : base("^") { }

        public override string GetDerivative()
        {
            if(Right.GetType() == typeof(NumberNode))
            {
                double rv = double.Parse(Right.GetValue());
                if (rv == 2)
                {
                    return rv + "*(" + Left.GetValue() + ")*(" + Left.GetDerivative() + ")";
                }
                else if (rv == 1) return "1";
                return rv + "*(" + Left.GetValue() + ")^" + (rv - 1) + "(" + Left.GetDerivative() + ")";
            }

            return Right.GetValue() + "*(" + Left.GetValue() + ")^(" + Right.GetValue() + "-1)*" + Left.GetDerivative();
        }

        public override string GetValue()
        {
            return Right.GetValue() + "^" + Left.GetValue();
        }
    }
}
