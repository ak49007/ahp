using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace AHP
{
    
    class GenSubsets
    {
        public int n { get; set; }
        public int row { get; set; }
        public int[,] bit;

        public GenSubsets(int n)
        {
            this.n = n;
            row = (int)Math.Pow(2, n) - 1;
            bit = new int[row, n];

            int curRow = 0;
            for (int i = 0; i < n; i++)
            {
                GenCombination g = new GenCombination(i+1, n);
                g.MethodGen();
                g.GenBit();
                int[,] tmp = g.getResult();

                for (int j = 1; j <= g.Row; j++)
                {
                    for (int k = 1; k <= n; k++)
                    {
                        bit[curRow, k - 1] = tmp[j, k];
                    }
                    curRow++;
                }

            }
        }

        public int[,] getResult()
        {
            return (int[,])bit.Clone();
        }
    }
}
