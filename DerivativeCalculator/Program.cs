using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a function");
            string fnc = "2*x + 3*x";

            Console.WriteLine(DerivativeSolver.GetDerivative(fnc));

            Console.Read();
        }
    }
}
