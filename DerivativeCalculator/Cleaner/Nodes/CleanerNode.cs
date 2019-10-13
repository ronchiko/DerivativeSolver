using System;
using System.Text;

namespace DerivativeCalculator
{
    public class CleanerNode : IComputableCleanerNode
    {
        public static readonly Type CN_TYPE = typeof(CleanerNode);

        private Identifier[] identifiers;
        private float value;

        private ICleanerNode powerNode;

        public float GetValue() => value;

        public CleanerNode(float value, Identifier[] identifiers)
        {
            this.value = value;
            this.identifiers = identifiers;
        }
        public CleanerNode(float value, Identifier[] identifiers, ICleanerNode pn)
        {
            this.value = value;
            this.identifiers = identifiers;
            powerNode = pn;
        }

        public static bool CanAdd(CleanerNode a, CleanerNode b){
            return Identifier.CanAdd(a.identifiers, b.identifiers);
        }

        public bool IsEqual(ICleanerNode n)
        {
            if (n.GetType() != typeof(CleanerNode)) return false;

            CleanerNode cnn = (CleanerNode)n;

            if (value != cnn.value || identifiers.Length != cnn.identifiers.Length)
                return false;

            for (int i = 0; i < identifiers.Length; i++)
            {
                if (!identifiers[i].IsEqual(cnn.identifiers[i])) return false;
            }

            return true;
        }

        public static CleanerNode operator *(CleanerNode a, CleanerNode b)
        {
            if(b.powerNode == null)
                return new CleanerNode(a.value * b.value, Identifier.Multiply(a.identifiers,b.identifiers));
            if (a.powerNode == null)
                return new CleanerNode(a.value * b.value, Identifier.Multiply(a.identifiers, b.identifiers));
            return new CleanerNode(a.value * b.value, Identifier.Multiply(a.identifiers, b.identifiers), null);
        }
        public static CleanerNode operator /(CleanerNode a, CleanerNode b)
        {
            if (b.powerNode == null)
                return new CleanerNode(a.value / b.value, Identifier.Divide(a.identifiers, b.identifiers));
            if (a.powerNode == null)
                return new CleanerNode(a.value / b.value, Identifier.Divide(a.identifiers, b.identifiers));
            return new CleanerNode(a.value / b.value, Identifier.Divide(a.identifiers, b.identifiers), null);
        }

        public static CleanerNode operator +(CleanerNode a, CleanerNode b)
        {
            if (!CanAdd(a, b)) throw new Exception(string.Format("Cannot add {0} and {1}.", a.ToString(), b.ToString()));
            return new CleanerNode(a.value + b.value, a.identifiers);
        }
        public static CleanerNode operator -(CleanerNode a, CleanerNode b)
        {
            if (!CanAdd(a, b)) throw new Exception(string.Format("Cannot add {0} and {1}.", a.ToString(), b.ToString()));

            return new CleanerNode(a.value - b.value, a.identifiers);
        }

        public static CleanerNode operator ^(CleanerNode a, CleanerNode b)
        {
            if(b.identifiers.Length == 0)
            {
                return new CleanerNode(a.value,Identifier.Pow(a.identifiers,b.value));
            }

            CleanerNode n = new CleanerNode(a.value, a.identifiers);
            for (int i = 0; i < a.identifiers.Length; i++)
            {
                n.identifiers[i] = new Identifier(a.identifiers[i].Base, 
                    string.Format("{0}*{1}",a.identifiers[i].Power,b.ToString()));
            }
            return n;
        }
        public static CleanerNode operator ^(CleanerNode a, ComplexCleanerNode b)
        {
            CleanerNode n = new CleanerNode(a.value, a.identifiers);
            for (int i = 0; i < a.identifiers.Length; i++)
            {
                n.identifiers[i] = new Identifier(a.identifiers[i].Base,
                    string.Format("{0}*{1}", a.identifiers[i].Power, b.ToString()));
            }
            return n;
        }

        public static CleanerNode operator -(CleanerNode a)
        {
            return new CleanerNode(-a.value, a.identifiers);
        }

        public override string ToString()
        {
            if (value == 0) return "0";
            if (value == 1 && identifiers.Length == 0) return "1";
            StringBuilder sb = new StringBuilder();
            if(value != 1) sb.Append(value);
            for (int i = 0; i < identifiers.Length; i++)
            {
                identifiers[i].CleanPower();
                sb.Append(identifiers[i]);
            }

            /*if(powerNode != null)
            {
                if(powerNode.GetType() == typeof(CleanerNode))
                    sb.Append(string.Format("^{0}", powerNode));
                else
                    sb.Append(string.Format("^({0})", powerNode));
            }*/

            return sb.ToString();
        }

        public ICleanerNode Multiply(ICleanerNode a)
        {
            if(typeof(CleanerNode) == a.GetType())
            {
                return (CleanerNode)a * this;
            }else if(typeof(ComplexCleanerNode) == a.GetType())
            {
                return ((ComplexCleanerNode)a).Multiply(this);
            }
            throw new NotImplementedException();
        }

        public ICleanerNode Add(ICleanerNode a)
        {
            if (CN_TYPE == a.GetType())
            {
                if(!CanAdd(this, (CleanerNode)a))
                    return new ComplexCleanerNode(this, new CleanerOperatorNode('+'), a);
                return (CleanerNode)a + this;
            }
            else if(typeof(ComplexCleanerNode) == a.GetType())
            {
                return ((ComplexCleanerNode)a).Add(this);
            }
            throw new NotImplementedException();
        }

        public ICleanerNode Sub(ICleanerNode a)
        {
            if (CN_TYPE == a.GetType())
            {
                if (!CanAdd(this, (CleanerNode)a))
                    return new ComplexCleanerNode(this, new CleanerOperatorNode('+'), -(CleanerNode)a);
                return this - (CleanerNode)a;
            }
            else if (typeof(ComplexCleanerNode) == a.GetType())
            {
                return ((IComputableCleanerNode)((ComplexCleanerNode)a).Multiply(new CleanerNode(1,CleanerOperatorNode.NO_ID))).Add(this);
            }
            throw new NotImplementedException();
        }

        public ICleanerNode Divide(ICleanerNode b)
        {
            if (CN_TYPE == b.GetType())
            {
                return this / (CleanerNode)b;
            }else if(typeof(ComplexCleanerNode) == b.GetType())
            {
                return new ComplexCleanerNode(this, new CleanerOperatorNode('/'), b);
            }
            throw new NotImplementedException();
        }

        public ICleanerNode Power(ICleanerNode a)
        {
            if(a.GetType() == typeof(CleanerNode))
            {
                return this ^ (CleanerNode)a;
            }else if(a.GetType() == typeof(ComplexCleanerNode))
            {              
                return this ^ (ComplexCleanerNode)a;
            }
            throw new NotImplementedException();
        }
    }
}