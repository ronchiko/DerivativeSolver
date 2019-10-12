namespace DerivativeCalculator
{
    public interface ICleanerNode
    {
        
    }

    /// <summary>
    /// A cleaner node that can be arithmaticly modified
    /// </summary>
    public interface IComputableCleanerNode : ICleanerNode
    {
        ICleanerNode Multiply(ICleanerNode a);
        ICleanerNode Add(ICleanerNode a);
        ICleanerNode Sub(ICleanerNode a);
        ICleanerNode Divide(ICleanerNode a);
        ICleanerNode Power(ICleanerNode a);
    }
}
