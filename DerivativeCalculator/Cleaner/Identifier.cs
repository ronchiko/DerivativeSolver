using System;
using System.Collections.Generic;

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
            if (a.Length != b.Length) return false;

            Array.Sort(a);
            Array.Sort(b);

            int iterateLess = Math.Min(a.Length, b.Length);

            for (int i = 0; i < iterateLess; i++)
            {
                if (!a[i].IsEqual(b[i])) return false;
            }

            return true;
        }

        public static Identifier[] Multiply(Identifier[] a, Identifier[] b)
        {
            List<Identifier> ids = new List<Identifier>();
            List<Identifier> pid = new List<Identifier>();
            pid.AddRange(a);
            pid.AddRange(b);

            while (pid.Count > 0)
            {
                string _bt = pid[0].Base;
                float _bp = pid[0].Power;
                for (int i = 1; i < pid.Count; i++)
                {
                    if(_bt == pid[i].Base)
                    {
                        _bp += pid[i].Power;
                        pid.RemoveAt(i);
                        i--;
                    }
                }
                ids.Add(new Identifier(_bt, _bp));
                pid.RemoveAt(0);
            }

            return ids.ToArray();
        }
        public static Identifier[] Divide(Identifier[] a, Identifier[] b)
        {
            List<Identifier> ids = new List<Identifier>();
            List<Identifier> pid = new List<Identifier>();
            pid.AddRange(a);
            pid.AddRange(b);

            while (pid.Count > 0)
            {
                string _bt = pid[0].Base;
                float _bp = pid[0].Power;
                for (int i = 1; i < pid.Count; i++)
                {
                    if (_bt == pid[i].Base)
                    {
                        _bp -= pid[i].Power;
                        pid.RemoveAt(i);
                        i--;
                    }
                }
                ids.Add(new Identifier(_bt, _bp));
                pid.RemoveAt(0);
            }

            return ids.ToArray();
        }

        public static Identifier[] Pow(Identifier[] a, float b)
        {
            Identifier[] r = new Identifier[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                r[i] = new Identifier(a[i].Base, a[i].power * b);
            }
            return r;
        }
        public override string ToString()
        {
            if (power == 1) return _base;
            if (power == 0) return "1";
            return _base + "^" + power;
        }
    }
}
