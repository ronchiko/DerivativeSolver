namespace DerivativeCalculator
{
    internal interface ICleanerNode
    {
        bool IsEqual(ICleanerNode other);
    }

    /// <summary>
    /// A cleaner node that can be arithmaticly modified
    /// </summary>
    internal interface IComputableCleanerNode : ICleanerNode
    {
        ICleanerNode Multiply(ICleanerNode a);
        ICleanerNode Add(ICleanerNode a);
        ICleanerNode Sub(ICleanerNode a);
        ICleanerNode Divide(ICleanerNode a);
        ICleanerNode Power(ICleanerNode a);
    }
}
