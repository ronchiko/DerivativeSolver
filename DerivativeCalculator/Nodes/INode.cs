namespace DerivativeCalculator
{
    internal interface INode
    {
        string Value { get; set; }
        INode[] Children { get; }
        string GetDerivative();
        string GetValue();
    }
}
