using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeCalculator
{
    internal class NumberNode : INode
    {
        public string Value { get; set; }
        public INode[] Children { get; private set; }

        public NumberNode(string value)
        {
            Value = value;
            Children = new INode[0];
        }

        public string GetDerivative()
        {
            return "0";
        }

        public string GetValue() => Value;
    }
}
