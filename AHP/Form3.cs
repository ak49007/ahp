using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHP
{
    public partial class Form3 : Form
    {
        private Element goal;
        private List<Element> alternatives;

        private Hashtable Bel = new Hashtable();
        private Hashtable Pl = new Hashtable();
        
        public Form3()
        {
            InitializeComponent();
        }

        public Form3(Element goal, List<Element> alternatives)
        {
            InitializeComponent();

            this.AutoScroll = true;
            this.goal = goal;
            this.alternatives = alternatives;

            calcBelPl();

            ShowResult();         
        }

        private void ShowResult()
        {
            int i = 0;
            try
            {
                Label lb_title1 = new Label();
                lb_title1.Size = new Size(100, 20);
                lb_title1.Location = new Point(50, 30);
                lb_title1.TextAlign = ContentAlignment.MiddleCenter;
                lb_title1.BorderStyle = BorderStyle.FixedSingle;
                lb_title1.Text = "Alternatives";
                panel2.Controls.Add(lb_title1);

                Label lb_title2 = new Label();
                lb_title2.Size = new Size(100, 20);
                lb_title2.Location = new Point(150, 30);
                lb_title2.TextAlign = ContentAlignment.MiddleCenter;
                lb_title2.BorderStyle = BorderStyle.FixedSingle;
                lb_title2.Text = "Bel";
                panel2.Controls.Add(lb_title2);

                Label lb_title3 = new Label();
                lb_title3.Size = new Size(100, 20);
                lb_title3.Location = new Point(250, 30);
                lb_title3.TextAlign = ContentAlignment.MiddleCenter;
                lb_title3.BorderStyle = BorderStyle.FixedSingle;
                lb_title3.Text = "Pl";
                panel2.Controls.Add(lb_title3);

                foreach (Element alte in alternatives)
                {
                    string key = alte.id + " ";

                    Label lb_name = new Label();
                    lb_name.Size = new Size(100, 20);
                    lb_name.Location = new Point(50, i * 20 + 50);
                    lb_name.BorderStyle = BorderStyle.FixedSingle;
                    lb_name.Text = alte.Text;
                    panel2.Controls.Add(lb_name);

                    TextBox tb_bel = new TextBox();
                    tb_bel.Size = new Size(100, 20);
                    tb_bel.Location = new Point(150, i * 20 + 50);
                    tb_bel.Text = Bel[key] + "";
                    panel2.Controls.Add(tb_bel);

                    TextBox tb_pl = new TextBox();
                    tb_pl.Size = new Size(100, 20);
                    tb_pl.Location = new Point(250, i * 20 + 50);
                    tb_pl.Text = Pl[key] + "";
                    panel2.Controls.Add(tb_pl);

                    i++;
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void calcBelPl()
        {
            try
            {
                Set setGoal = new Set();
                setGoal.Add(goal);
                Bel.Add(setGoal.Name, 1);
                Pl.Add(setGoal.Name, 1);

                Element cur = goal;
                while (cur.next.Count != 0)
                {
                    CalcBelPl(cur.next);
                    cur = cur.next[0];
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void CalcBelPl(List<Element> list)
        {
            try
            {
                GenSubsets g = new GenSubsets(list.Count);
                int[,] bit = g.getResult();

                for (int i = 0; i < g.row; i++)
                {
                    Set tmp = new Set();
                    for (int j = 0; j < g.n; j++)
                    {
                        if (bit[i, j] == 1)
                            tmp.Add(list[j]);
                    }

                    CalcPl(tmp);
                    CalcBel(tmp);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void CalcPl(Set set)
        {
            ArrayList hs = new ArrayList();
            ArrayList right = new ArrayList();
            ArrayList left = new ArrayList();

            try
            {
                int n = set[0].prev.Count;

                foreach (var item in set[0].prev)
                {
                    hs.Add((double)item.Pl_next[set.Name]);
                }

                GenSubsets g = new GenSubsets(n);
                int[,] bit = g.getResult();

                for (int i = 0; i < g.row; i++)
                {
                    Set tmp = new Set();
                    for (int j = 0; j < g.n; j++)
                    {
                        if (bit[i, j] == 1)
                            tmp.Add(set[0].prev[j]);
                    }

                    right.Add(Pl[tmp.Name]);
                    left.Add(Bel[tmp.Name]);
                }

                Simplex sim = new Simplex(n, hs, right, left);
                sim.Run();
                double res = sim.a[sim.N - 1, sim.M - 1];
                Pl.Add(set.Name, res);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void CalcBel(Set set)
        {
            ArrayList hs = new ArrayList();
            ArrayList right = new ArrayList();
            ArrayList left = new ArrayList();

            try
            {
                int n = set[0].prev.Count;

                foreach (var item in set[0].prev)
                {
                    hs.Add(-1 * (double)item.Bel_next[set.Name]);
                }

                GenSubsets g = new GenSubsets(n);
                int[,] bit = g.getResult();

                for (int i = 0; i < g.row; i++)
                {
                    Set tmp = new Set();
                    for (int j = 0; j < g.n; j++)
                    {
                        if (bit[i, j] == 1)
                            tmp.Add(set[0].prev[j]);
                    }

                    right.Add(Pl[tmp.Name]);
                    left.Add(Bel[tmp.Name]);
                }

                Simplex sim = new Simplex(n, hs, right, left);
                sim.Run();
                double res = -sim.a[sim.N - 1, sim.M - 1];
                Bel.Add(set.Name, res);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
