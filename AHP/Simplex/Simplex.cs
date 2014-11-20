using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace AHP
{
    public class Simplex
    {
        public int N, M;
        public double[,] a;
        public double[,] b;
        public double[] p0;
        public bool ok;
        public bool ok2;
        public Simplex(int n, ArrayList hs, ArrayList right, ArrayList left)
        {
            ok = true;
            ok2 = true;
            N = 2 * ((int)Math.Pow(2, n) - 2) + 3;
            M = n + 2 * ((int)Math.Pow(2, n) - 1) + 1;
            a = new double[N, M];
            b = new double[N, M];
            p0 = new double[N - 2];
            Generated gen = new Generated(n);
            for (int j = n - 1; j > 0; j--)
            {
                gen.Sinh(j);
            }
            for (int j = n - 1; j > 0; j--)
            {
                gen.Sinh(j);
            }
            gen.Run();
            for (int i = 0; i < N - 3; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a[i, j] = -(double)gen.A1[i, j];
                }
            }
            for (int j = 0; j < n; j++)
            {
                a[N - 3, j] = 1.0;
            }
            for (int j = n; j < n + (int)Math.Pow(2, n) - 2; j++)
            {
                for (int i = 0; i < N - 3; i++)
                {
                    if (i - j == (int)Math.Pow(2, n) - 2 - n)
                    {
                        a[i, j] = -1.0;
                    }
                    else
                    {
                        a[i, j] = 0.0;
                    }
                }
            }
            for (int j = n + (int)Math.Pow(2, n) - 2; j < M - 1; j++)
            {
                for (int i = 0; i < N - 3; i++)
                {
                    if (j - i == n + (int)Math.Pow(2, n) - 2)
                    {
                        a[i, j] = 1.0;
                    }
                    else
                    {
                        a[i, j] = 0.0;
                    }
                }
            }
            for (int j = n; j < M - 1; j++)
            {
                a[N - 3, j] = 0.0;
            }
            //b
            for (int i = 0; i < (int)Math.Pow(2, n) - 2; i++)
            {
                a[i, M - 1] = (double)right[i];
                a[((int)Math.Pow(2, n) - 2 + i), M - 1] = (double)left[i];
            }
            a[N - 3, M - 1] = 1;
            // hs
            for (int i = 0; i < (int)Math.Pow(2, n) - 2; i++)
            {
                p0[i] = 0.0;
            }
            for (int i = (int)Math.Pow(2, n) - 2; i < N - 2; i++)
            {
                p0[i] = -1.0;
            }
            for (int j = 0; j < n; j++)
            {
                double t = 0.0;
                for (int i = 0; i < N - 2; i++)
                {
                    t += p0[i] * a[i, j];
                }
                a[N - 2, j] = t;
                a[N - 1, j] = -(double)hs[j];
            }
            for (int j = n; j < M; j++)
            {
                double t = 0.0;
                for (int i = 0; i < N - 2; i++)
                {
                    t += p0[i] * a[i, j];
                }
                a[N - 2, j] = t;
                a[N - 1, j] = 0.0;
            }
            GanB();
        }        
        public void GanB()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    b[i, j] = a[i, j];
                }
            }
        }
        public int ChonCot()
        {
            int id = -1;
            double min = double.MaxValue;
            for (int j = 0; j < M - 1; j++)
            {
                if (a[N - 2, j] < 0 && a[N - 2, j] < min)
                {
                    min = a[N - 2, j];
                    id = j;
                }
            }
            return id;
        }
        public int ChonHang()
        {
            double min = double.MaxValue;
            int id = -1;
            int J = ChonCot();
            for (int i = 0; i < N - 2; i++)
            {
                double d = (double)a[i, M - 1] / (double)a[i, J];
                if (d > 0 && d < min)
                {
                    min = d;
                    id = i;
                }
            }
            return id;
        }
        public void DoiMaTran(int I, int J)
        {
            for (int j = 0; j < M; j++)
            {
                a[I, j] = (double)b[I, j] / b[I, J];
            }
            for (int i = 0; i < N; i++)
            {
                if (i != I)
                {
                    a[i, J] = 0.0;
                }
            }
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                {
                    if (i != I && j != J)
                    {
                        a[i, j] = (double)b[i, j] - (double)b[I, j] * b[i, J] / b[I, J];
                    }
                }
            GanB();
        }
        public void Working()
        {
            int j = ChonCot();
            if (j == -1)
            {
                ok = false;
                return;
            }
            else
            {
                int i = ChonHang();
                if (i != -1)
                {
                    DoiMaTran(i, j);
                }
                else
                {
                    ok = false;
                    return;
                }

            }
        }
        // pha hai
        public int ChonCol()
        {
            int id = -1;
            double min = double.MaxValue;
            for (int j = 0; j < M - 1; j++)
            {
                if (a[N - 1, j] < 0 && a[N - 1, j] < min)
                {
                    min = a[N - 1, j];
                    id = j;
                }
            }
            return id;
        }
        public int ChonRow()
        {
            double min = double.MaxValue;
            int id = -1;
            int J = ChonCol();
            for (int i = 0; i < N - 2; i++)
            {
                double d = (double)a[i, M - 1] / (double)a[i, J];
                if (d > 0 && d < min)
                {
                    min = d;
                    id = i;
                }
            }
            return id;
        }
        public void Working2()
        {
            int j = ChonCol();
            if (j == -1)
            {
                ok2 = false;
                return;
            }
            else
            {
                int i = ChonRow();
                if (i != -1)
                {
                    DoiMaTran(i, j);
                }
                else
                {
                    ok2 = false;
                    return;
                }

            }
        }
        public void Print()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    Console.Write(a[i, j].ToString() + "\t");
                }
                Console.WriteLine();
            }
        }
        public void Run()
        {
            while (ok)
            {
                Working();               
            }
            while (ok2)
            {
                Working2();
            }
        }
    }
}
