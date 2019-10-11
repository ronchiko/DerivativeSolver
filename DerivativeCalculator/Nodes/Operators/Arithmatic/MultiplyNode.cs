using System;
namespace DerivativeCalculator
{
    internal class MultiplyNode : OperatorNode
    {
        public MultiplyNode() : base("*") { }

        public override string GetDerivative()
        {
            return Left.GetDerivative() + "*" + Right.GetDerivative();
        }
    }
}
