using System.Collections.Generic;
using System.Linq;

namespace DerivativeCalculator
{
    public class ComplexCleanerNode : IComputableCleanerNode
    {
        private List<ICleanerNode> subNodes;
        private IComputableCleanerNode powerNode;
        public ComplexCleanerNode(params ICleanerNode[] nodes)
        {
            powerNode = null;
            subNodes = nodes.ToList();
        }

        public bool IsEqual(ICleanerNode n)
        {
            if (n == null) return false;

            if (n.GetType() == typeof(ComplexCleanerNode)) {

                ComplexCleanerNode ccnn = (ComplexCleanerNode)n;
                if (ccnn.subNodes.Count != subNodes.Count ||
                    (ccnn.powerNode != null && ccnn.powerNode.IsEqual(powerNode))
                    || (ccnn.powerNode == null && powerNode != null)) return false; 

                for (int i = 0; i < subNodes.Count; i++)
                {
                    if (!subNodes[i].IsEqual(ccnn.subNodes[i]))
                        return false;
                }

                return true;
            }
            return false;
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
            return new ComplexCleanerNode(this, new CleanerOperatorNode('/'), a);
        }

        public ICleanerNode Multiply(ICleanerNode a)
        {
            if (typeof(ComplexCleanerNode) == a.GetType())
            {
                ComplexCleanerNode cnna = (ComplexCleanerNode)a;

                if (cnna.IsEqual(this))
                {
                    ComplexCleanerNode n = new ComplexCleanerNode(subNodes.ToArray());

                    if (cnna.powerNode == null && powerNode == null)
                        n.powerNode = new CleanerNode(2, CleanerOperatorNode.NO_ID);
                    else if (cnna.powerNode == null)
                        n.powerNode = (IComputableCleanerNode)powerNode.Add(new CleanerNode(1, CleanerOperatorNode.NO_ID));
                    else if (powerNode == null)
                        n.powerNode = (IComputableCleanerNode)cnna.powerNode.Add(new CleanerNode(1, CleanerOperatorNode.NO_ID));
                    else
                        n.powerNode = (IComputableCleanerNode)powerNode.Add(cnna.powerNode);
                    return n;
                }

                if ((powerNode == null && cnna.powerNode == null) || 
                    (powerNode != null && !powerNode.IsEqual(cnna.powerNode)) ||
                    (powerNode == null && cnna.powerNode != null))
                {
                    return new ComplexCleanerNode(this,new CleanerOperatorNode('*'),cnna);
                }

                cnna.IterateOverComputables((IComputableCleanerNode node, int i) =>
                {
                    Multiply(node);
                });
                return this;
            }
            else if (typeof(CleanerNode) == a.GetType())
            {
                if(powerNode != null)
                {
                    return new ComplexCleanerNode(this,new CleanerOperatorNode('*'),a);
                }

                if (subNodes[1].GetType() == typeof(CleanerOperatorNode)) {
                    if (((CleanerOperatorNode)subNodes[1]).type == '*' ||
                        ((CleanerOperatorNode)subNodes[1]).type == '/')
                    {
                        for (int i = 0; i < subNodes.Count; i++)
                        {
                            if (!typeof(IComputableCleanerNode).IsAssignableFrom(subNodes[i].GetType()))
                                continue;
                            IComputableCleanerNode node = (IComputableCleanerNode)subNodes[i];
                            subNodes[i] = node.Multiply((CleanerNode)a);
                            break;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < subNodes.Count; i++)
                        {
                            if (!typeof(IComputableCleanerNode).IsAssignableFrom(subNodes[i].GetType()))
                                continue;
                            IComputableCleanerNode node = (IComputableCleanerNode)subNodes[i];
                            subNodes[i] = node.Multiply((CleanerNode)a);
                        }
                    }
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

            ICleanerNode pn = null, nn = null;

            for (int i = 0; i < subNodes.Count; i++)
            {
                nn = i < subNodes.Count - 1 ? subNodes[i + 1] : null;

                if (pn != null)
                {
                    if(pn.GetType() == typeof(CleanerOperatorNode))
                    {
                        if (((CleanerOperatorNode)pn).type == '*')
                        {
                            sb.Remove(sb.Length - 1, 1);
                            if (subNodes[i].GetType() == typeof(ComplexCleanerNode))
                            {
                                sb.Append(string.Format("({0})", subNodes[i]));
                            }
                            else
                            {
                                sb.Append(subNodes[i]);
                            }
                        }
                        else if (((CleanerOperatorNode)pn).type == '/')
                        {
                            if (subNodes[i].GetType() == typeof(ComplexCleanerNode))
                            {
                                sb.Append(string.Format("({0})", subNodes[i]));
                            }
                            else
                            {
                                sb.Append(subNodes[i]);
                            }
                        }
                        else
                        {
                            sb.Append(subNodes[i]);
                        }
                    }
                    else
                    {
                        sb.Append(subNodes[i].ToString());
                    }
                }
                else
                {
                    if(nn != null && nn.GetType() == typeof(CleanerOperatorNode))
                    {
                        if(((CleanerOperatorNode)nn).type == '*' ||
                            ((CleanerOperatorNode)nn).type == '/')
                        {
                            if (subNodes[i].GetType() == typeof(ComplexCleanerNode))
                                sb.Append(string.Format("({0})", subNodes[i]));
                            else
                                sb.Append(subNodes[i]);
                        }
                        else
                        {
                            sb.Append(subNodes[i].ToString());
                        }
                    }else
                        sb.Append(subNodes[i].ToString());
                }

                pn = subNodes[i];
            }

            if (powerNode != null) {
                sb.Insert(0, "(");
                sb.Append(")^");
                if (powerNode.GetType() == typeof(ComplexCleanerNode))
                {
                    sb.Append(string.Format("({0})", powerNode.ToString()));
                } else if (powerNode.GetType() == typeof(CleanerNode))
                {
                    sb.Append(powerNode.ToString());
                }
            }

            return sb.ToString();
        }

        public ICleanerNode Power(ICleanerNode a)
        {
            if (!typeof(IComputableCleanerNode).IsAssignableFrom(a.GetType()))
                throw new System.Exception("Uncomputable node type " + a.GetType().Name);
            powerNode = (IComputableCleanerNode)a;
            return this;
        }
    }
}
