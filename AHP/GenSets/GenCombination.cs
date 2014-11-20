using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHP
{
    class GenCombination
    {
        private int k;
        private int n;
        private int[] a;
        public int id = 0;
        public int[,] b;
        public int[,] bit;
        private int row;
        public int K
        {
            get
            {
                return k;
            }
            set
            {
                k = value;
            }
        }
        public int N
        {
            get
            {
                return n;

            }
            set
            {
                n = value;
            }
        }
        public int Row
        {
            get {
                return row;
            }
            set {
                row = value;
            }
        }
        public GenCombination(int k, int n)
        {
            this.K = k;
            this.N = n;
            this.Row = GenCombination.fact(n) / (GenCombination.fact(k) * GenCombination.fact(n-k));
            a = new int[this.K+1];
            b = new int[this.Row+1, this.K + 1];
            bit = new int [this.Row + 1, this.N+1];
        }
        public void Init()
        {
            for (int i = 1; i <= this.K; i++)
            {
                a[i] = i;
            }
            
        }
        public void Out()
        {
           for (int i = 1; i <= this.K; i++)
            {
                b[id, i] = a[i];
            }           
            
        }
        public bool Islast()
        {
            for (int i = 1; i <= this.K; i++)
            {
                if (a[i] != this.N - this.K + i) return false;
            }
            return true;
        }
        public void Gen()
        {
            int i = this.K;
            while (a[i] == this.N - this.K + i) i--;
            a[i] = a[i] + 1;
            for (int j = i + 1; j <= this.K; j++)
            {
                a[j] = a[i] + j - i;
            }
           
        }
        public void Print()
        {
            for (int i = 1 ; i <= this.Row; i++)
            {
                for (int j = 1; j <= this.K; j++)
                {
                    Console.Write("{0} ", b[i, j]);   
                }
                Console.WriteLine();
                
            }
        }
        public static int fact(int n)
        {
            int f = 1;
            for (int i = 1; i <= n; i++)
            {
                f = f * i;
            }
            return f;
        }
        public void MethodGen()
        {
            Init();
            for (int i = 1; i <= this.K; i++)
            {
                b[1, i] = a[i];

            }
            bool stop = Islast();
            int id = 2;
            while (stop == false)
            {
                Gen();

                stop = Islast();
                for (int i = 1; i <= this.K; i++)
                {
                    b[id, i] = a[i];

                }
                id++;

            }

        }
        public void GenBit()
        {
            for (int k = 1; k <= this.N; k++)
            {
                for (int i = 1; i <= this.Row; i++)
                {
                    for (int j = 1; j <= this.K; j++)
                    {
                           if (b[i, j] == k)
                            {
                                bit[i, k] = 1;
                            }                          
                    }
                }
            }
        }

        public int[,] getResult()
        {
            return (int[,])bit.Clone();
        }
    }
}
