using System;
using System.Collections.Generic;

namespace DerivativeCalculator
{
    public struct Identifier
    {
        private string _base;
        private string power;

        public string Base => _base;
        public string Power => power;

        public Identifier(string b, float p)
        {
            power = p.ToString();
            _base = b;
        }
        public Identifier(string b, string p)
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

        public void CleanPower()
        {
            power = Cleaner.Clean(power);
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
                string _bp = pid[0].Power;
                for (int i = 1; i < pid.Count; i++)
                {
                    if(_bt == pid[i].Base)
                    {
                        float p, np;
                        if(float.TryParse(_bp, out p)
                            && float.TryParse(pid[i].power, out np))
                        {
                            _bp = (p + np).ToString();
                        }
                        else
                        {
                            _bp += string.Format("+{0}", pid[i].Power);
                            Console.WriteLine(_bp);
                        }

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
                string _bp = pid[0].Power;
                for (int i = 1; i < pid.Count; i++)
                {
                    if (_bt == pid[i].Base)
                    {
                        float p, np;
                        if (float.TryParse(_bp, out p) && float.TryParse(pid[i].Power, out np))
                        {
                            
                            _bp = (p - np).ToString();
                        }
                        else
                        {
                            _bp += string.Format("-{0}", pid[i].Power);
                        }
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
                float p;
                if(float.TryParse(a[i].Power, out p))
                {
                    r[i] = new Identifier(a[i].Base, p * b);
                }
                else
                {
                    r[i] = new Identifier(a[i].Base, string.Format("{0}*{1}",a[i].power , b));
                }
                
            }
            return r;
        }
        public override string ToString()
        {
            float p;
            if (float.TryParse(power, out p))
            {
                if (p == 1) return _base;
                if (p == 0) return "1";
            }
            if (power.Length == 1) return _base + "^" + power;
            return _base + "^(" + power + ")";
        }
    }
}
