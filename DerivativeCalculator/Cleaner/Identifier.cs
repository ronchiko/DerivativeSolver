using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerivativeCalculator
{
    public struct Identifier
    {
        private string _base;
        private float power;

        public string Base => _base;
        public float Power => power;

        public Identifier(string b, float p)
        {
            power = p;
            _base = b;
        }

        public bool IsEqual(Identifier id)
        {
            return id._base == _base && id.power == power;
        }

        public static bool CanAdd(Identifier[] a, Identifier[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                int count = 0;
                for (int j = 0; j < b.Length; j++)
                {
                    if (a[i].IsEqual(b[j]))
                    {
                        count++;
                    }
                }
                if (count != 1) return false;
            }
            return true;
        }

        public static Identifier[] Multiply(Identifier[] a, Identifier[] b)
        {
            List<Identifier> ids = new List<Identifier>();
            for (int i = 0; i < a.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < b.Length && !found; j++)
                {
                    if (b[j].Base == a[i].Base)
                    {
                        ids.Add(new Identifier(b[j].Base, b[j].power + a[i].Power));
                        found = true;
                    }
                }
                if (!found) ids.Add(a[i]);
            }
            return ids.ToArray();
        }
        public static Identifier[] Divide(Identifier[] a, Identifier[] b)
        {
            List<Identifier> ids = new List<Identifier>();
            for (int i = 0; i < a.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < b.Length; j++)
                {
                    if (b[j].Base == a[i].Base)
                    {
                        if(a[j].power - b[i].Power != 0) ids.Add(new Identifier(b[j].Base, a[j].power - b[i].Power));
                        found = true;
                    }
                }
                if (!found) ids.Add(a[i]);
            }

            return ids.ToArray();
        }

        public override string ToString()
        {
            if (power == 1) return _base;
            if (power == 0) return "1";
            return _base + "^" + power;
        }
    }
}
