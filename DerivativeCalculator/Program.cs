using System;

namespace DerivativeCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ArithmaticFunction function = new ArithmaticFunction("3x");

            Console.WriteLine(function.Function);
            Console.WriteLine(function.Derivative);
            Console.WriteLine(function.SecondDerivative);

            Console.WriteLine(ArithmaticFunction.SolveWithX(function.Function, 8));

            Console.Read();
        }
    }
}
