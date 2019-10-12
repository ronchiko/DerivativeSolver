using System;

namespace DerivativeCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            CleanerNode cn1 = new CleanerNode(2, new[]
            {
                new Identifier("x", 1),
                new Identifier("y", 1)
            });
            CleanerNode cn2 = new CleanerNode(2, new[]
            {
                new Identifier("x", 1),
                new Identifier("y", .5f)
            });
            Console.WriteLine(cn1 / cn2);

            Console.WriteLine("Enter a function");
            string fnc = "(3+2x)^2";

            Console.WriteLine(DerivativeSolver.GetDerivative(fnc));

            Console.Read();
        }
    }
}
