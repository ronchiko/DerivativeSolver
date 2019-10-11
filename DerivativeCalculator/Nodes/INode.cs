using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeCalculator
{
    internal interface INode
    {
        string Value { get; set; }
        INode[] Children { get; }
        string GetDerivative();
    }
}
