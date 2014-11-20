using System;
using System.Collections.Generic;
//using System.Text;
using System.Collections;

namespace AHP
{
    public class Generated
    {
        public int N;
        public ArrayList res;
        public double[,] A1;
        public Generated(int n)
        {
            N = n;
            res = new ArrayList();
            A1 = new double[2 * ((int)Math.Pow(2, N) - 2), N];
        }
        public ArrayList Sinh(ArrayList old)
        {
            ArrayList al = new ArrayList();
            int count = 0;
            for (int i = old.Count - 1; i >= 0; i--)
            {
                if ((int)old[i] == 0) count = count + 1;
                if ((int)old[i] == -1)
                {
                    if (count > 0)
                    {
                        al = Change(i, count - 1, old);
                        break;
                    }
                }
            }
            if (al.Count == 0) return null;
            return al;
        }
        ArrayList Change(int id, int count, ArrayList old)
        {
            ArrayList al = new ArrayList();
            for (int i = 0; i < id; i++)
            {
                al.Add(old[i]);
            }
            al.Add(0);
            int index = old.Count - count;
            for (int j = id + 1; j < index; j++)
            {
                al.Add(-1);
            }
            for (int j = index; j < old.Count; j++)
            {
                al.Add(0);
            }
            return al;
        }
        public void Sinh(int k)
        {
            ArrayList old = new ArrayList();
            for (int i = 0; i < N - k; i++) old.Add(-1);
            for (int i = N - k; i < N; i++) old.Add(0);
            res.Add(old);
            ArrayList al = new ArrayList();
            while (al != null)
            {
                al = Sinh(old);
                if (al != null) res.Add(al);
                old = al;
            }
        }
        public void Run()
        {
            int i = 0;
            foreach (ArrayList al in res)
            {
                int j = 0;
                foreach (int d in al)
                {
                    A1[i, j] = d;
                    j++;
                }
                i++;
            }
        }
        
    }

}
