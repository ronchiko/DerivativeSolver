using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeCalculator
{
    internal class AdditionNode : OperatorNode
    {
        public AdditionNode() : base("+") { }

        public override string GetDerivative()
        {
            return Left.GetDerivative() + "+" + Right.GetDerivative(); 
        }
    }
}
