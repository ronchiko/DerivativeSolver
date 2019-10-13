using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DerivativeCalculator
{
    public class ArithmaticFunction
    {
        private string derivative;
        private string cleanDerivative;

        private string secondDerivative;
        private string cleanSecondDerivative;

        private ArithmaticFunctionDerivativeMode derivativeMode;

        public string Function { get; }
        public string Derivative => cleanDerivative;
        public string SecondDerivative => cleanSecondDerivative;


        /// <summary>
        /// Creates a function and its first 2 derivatives
        /// </summary>
        /// <param name="function"></param>
        public ArithmaticFunction(string function)
        {
            Function = function;

            derivative = DerivativeSolver.GetDerivative(function);
            cleanDerivative = Cleaner.Clean(derivative);

            secondDerivative = DerivativeSolver.GetDerivative(derivative);
            cleanSecondDerivative = Cleaner.Clean(secondDerivative);

            derivativeMode = ArithmaticFunctionDerivativeMode.Two;
        }
        /// <summary>
        /// Creates a function and a given amount of derivatives
        /// </summary>
        /// <param name="function"></param>
        /// <param name="mode"></param>
        public ArithmaticFunction(string function, ArithmaticFunctionDerivativeMode mode)
        {
            Function = function;

            if (mode == ArithmaticFunctionDerivativeMode.One || mode == ArithmaticFunctionDerivativeMode.Two)
            {
                derivative = DerivativeSolver.GetDerivative(function);
                cleanDerivative = Cleaner.Clean(derivative);
            }

            if(mode == ArithmaticFunctionDerivativeMode.Two)
            {
                secondDerivative = DerivativeSolver.GetDerivative(derivative);
                cleanSecondDerivative = Cleaner.Clean(secondDerivative);
            }

            derivativeMode = mode;
        }

        // Evaluator function
        private static float Evaluate(float a, float b, char op)
        {
            switch (op)
            {
                case '+': return a + b;
                case '-': return a - b;
                case '*': return a * b;
                case '/': return a / b;
                case '^': return (float)Math.Pow(a, b);
            }

            throw new NotImplementedException();
        }
        /// <summary>
        /// Solve an expression while replacing the 'x'es to a given value
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float SolveWithX(string expression, float x)
        {
            expression = DerivativeSolver.FixFunctionExpression(expression).Replace("x", x.ToString());

            // Common variables
            Queue<string> q = new Queue<string>();
            Stack<string> s = new Stack<string>();

            //Perform shanting yard algorithem
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < expression.Length; i++)
            {
                if (Cleaner.Splitters.Contains(expression[i]))
                {
                    sb.Replace(" ", "");
                    if (sb.Length > 0)
                        q.Enqueue(sb.ToString());

                    switch (expression[i])
                    {
                        case '(':
                            s.Push("(");
                            break;
                        case ')':
                            while (s.Count > 0 && s.Peek() != "(")
                            {
                                q.Enqueue(s.Pop());
                            }
                            s.Pop();
                            break;
                        default:
                            byte ov = Cleaner.GetOV(expression[i]);
                            while (s.Count > 0 && Cleaner.GetOV(s.Peek()[0]) != 0 &&
                                Cleaner.GetOV(s.Peek()[0]) >= ov)
                            {
                                q.Enqueue(s.Pop());
                            }
                            s.Push(expression[i].ToString());
                            break;
                    }

                    sb.Clear();
                    continue;
                }

                sb.Append(expression[i]);
            }
            sb.Replace(" ", "");
            if (sb.Length > 0)
                q.Enqueue(sb.ToString());

            while (s.Count > 0)
            {
                q.Enqueue(s.Pop());
            }

            //Stack evaluator
            while (q.Count > 0)
            {
                string deq = q.Dequeue();
                // Is operator?
                if (Cleaner.Splitters.Contains(deq[0]))
                {
                    float a = float.Parse(s.Pop());
                    float b = s.Count > 0 ? float.Parse(s.Pop()) : 0;

                    s.Push(Evaluate(b, a, deq[0]).ToString());
                }
                else
                {
                    s.Push(deq);
                }
            }

            return float.Parse(s.Pop());
        }

        public float GetY(float x)
        {
            return SolveWithX(Function, x);
        }
        public float GetIncline(float x)
        {
            return SolveWithX(derivative, x);
        }
        public float GetDerivativeIncline(float x)
        {
            return SolveWithX(secondDerivative, x);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if(obj.GetType() == typeof(ArithmaticFunction))
            {
                return Function == ((ArithmaticFunction)obj).Function;
            }

            return false;
        }
        public override string ToString()
        {
            return Function;
        }
    }
}
