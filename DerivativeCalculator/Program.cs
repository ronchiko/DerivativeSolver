using System;

namespace DerivativeCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Cleaner.Clean("2*x^(x+2)+2*x*x"));

            Console.WriteLine("Enter a function");
            string fnc = "(3+2x)^2";

            Console.WriteLine(DerivativeSolver.GetDerivative(fnc));

            Console.Read();
        }
    }
}
