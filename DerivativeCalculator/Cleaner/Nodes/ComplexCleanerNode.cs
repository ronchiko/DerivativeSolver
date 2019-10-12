using System.Collections.Generic;
using System.Linq;

namespace DerivativeCalculator
{
    public class ComplexCleanerNode : IComputableCleanerNode
    {
        private List<ICleanerNode> subNodes;
        private ICleanerNode powerNode;
        public ComplexCleanerNode(params ICleanerNode[] nodes)
        {
            subNodes = nodes.ToList();
        }

        public void IterateOverComputables(System.Action<IComputableCleanerNode, int> action)
        {
            for (int i = 0; i < subNodes.Count; i++)
            {
                if (typeof(IComputableCleanerNode).IsAssignableFrom(subNodes[i].GetType()))
                {
                    action?.Invoke((IComputableCleanerNode)subNodes[i], i);
                }
            }
        }

        public List<IComputableCleanerNode> GetComputables()
        {
            List<IComputableCleanerNode> computables = new List<IComputableCleanerNode>();
            for (int i = 0; i < subNodes.Count; i++)
            {
                if (typeof(IComputableCleanerNode).IsAssignableFrom(subNodes[i].GetType()))
                {
                    computables.Add((IComputableCleanerNode)subNodes[i]);
                }
            }
            return computables;
        }

        public ICleanerNode Add(ICleanerNode a)
        {
            if(typeof(ComplexCleanerNode) == a.GetType())
            {
                
                ComplexCleanerNode ccna = ((ComplexCleanerNode)a);
                for (int j = 0; j < ccna.subNodes.Count; j++)
                {
                    if (!typeof(IComputableCleanerNode).IsAssignableFrom(ccna.subNodes[j].GetType()))
                        continue;
                    Add(ccna.subNodes[j]);
                }
                
                return this;    
            }else if (typeof(CleanerNode) == a.GetType())
            {
                bool added = false;

                for (int i = 0;i < subNodes.Count;i++)
                {
                    if (!typeof(IComputableCleanerNode).IsAssignableFrom(subNodes[i].GetType()))
                        continue;
                    IComputableCleanerNode node = (IComputableCleanerNode)subNodes[i];
                    if (CleanerNode.CanAdd((CleanerNode)node, (CleanerNode)a))
                    {
                        added = true;
                        subNodes[i] = (CleanerNode)node + (CleanerNode)a;
                        break;
                    }
                }

                if (!added)
                {
                    subNodes.Add(new CleanerOperatorNode('+'));
                    subNodes.Add(a);
                }

                return this;
            }
            throw new System.NotImplementedException();
        }

        public ICleanerNode Divide(ICleanerNode a)
        {           
            throw new System.NotImplementedException();
        }

        public ICleanerNode Multiply(ICleanerNode a)
        {
            if (typeof(ComplexCleanerNode) == a.GetType())
            {
                ((ComplexCleanerNode)a).IterateOverComputables((IComputableCleanerNode node, int i) =>
                {
                    Multiply(node);
                });

                return this;
            }
            else if (typeof(CleanerNode) == a.GetType())
            {
                for (int i = 0; i < subNodes.Count; i++)
                {
                    if (!typeof(IComputableCleanerNode).IsAssignableFrom(subNodes[i].GetType()))
                        continue;
                    IComputableCleanerNode node = (IComputableCleanerNode)subNodes[i];                 
                    subNodes[i] = (CleanerNode)node * (CleanerNode)a;                  
                }
                return this;
            }
            throw new System.NotImplementedException();
        }

        public ICleanerNode Sub(ICleanerNode a)
        {
            if (typeof(ComplexCleanerNode) == a.GetType())
            {
                ((ComplexCleanerNode)a).IterateOverComputables((IComputableCleanerNode node, int i) =>
                {
                    Sub(node);
                });

                return this;
            }
            else if (typeof(CleanerNode) == a.GetType())
            {
                for (int i = 0; i < subNodes.Count; i++)
                {
                    if (!typeof(IComputableCleanerNode).IsAssignableFrom(subNodes[i].GetType()))
                        continue;
                    IComputableCleanerNode node = (IComputableCleanerNode)subNodes[i];
                    if (CleanerNode.CanAdd((CleanerNode)node, (CleanerNode)a))
                    {
                        subNodes[i] = (CleanerNode)node - (CleanerNode)a;
                    }
                }
                return this;
            }
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < subNodes.Count; i++)
            {
                sb.Append(subNodes[i].ToString());
            }

            return sb.ToString();
        }

        public ICleanerNode Power(ICleanerNode a)
        {
            throw new System.NotImplementedException();
        }
    }
}
