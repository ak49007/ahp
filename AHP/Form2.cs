using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHP
{
    public partial class Form2 : Form
    {
        Element element;
        Label[] lb_sets; //ten cac tap hop

        TextBox[] tb_values;    //cac gia tri
        TextBox[] tb_Bel; 
        TextBox[] tb_Pl;

        int n;
        List<Set> sets;    //cac tap hop
        Hashtable bel = new Hashtable();
        Hashtable pl = new Hashtable();
        double[] m;

        public Form2()
        {
            InitializeComponent();
            tab_minmax.BackColor = Control.DefaultBackColor;
            n = 0;
        }

        public Form2(Element element)
        {
            InitializeComponent();

            tab_minmax.BackColor = Control.DefaultBackColor;
            panel2.AutoScroll = true;
            this.element = element;
            n = (int)Math.Pow(2,element.next.Count) - 1;

            sets = new List<Set>();
            GenSets();  //sinh cac tap hop

            lb_sets = new Label[n];
            tb_values = new TextBox[n];
            tb_Bel = new TextBox[n];
            tb_Pl = new TextBox[n];

            ShowTable();

            InitNamesOfSets();  //Thiet lap ten cac Label

            for (int i = 0; i < n; i++)
            {
                try
                {
                    tb_values[i].Text = "" + element.value.ElementAt(i);
                }
                catch {
                    tb_values[i].Text = "0";
                }
            }

            /*
            bel = new double[n];
            pl = new double[n];
             */
            m = new double[n];

            updateBelPl();
        }

        private void ShowTable()
        {
            Label lb_title1 = new Label();
            lb_title1.Size = new Size(110, 20);
            lb_title1.Location = new Point(0, 0);
            lb_title1.TextAlign = ContentAlignment.MiddleCenter;
            lb_title1.BorderStyle = BorderStyle.FixedSingle;
            lb_title1.Text = "Elements";
            panel2.Controls.Add(lb_title1);

            Label lb_title2 = new Label();
            lb_title2.Size = new Size(110, 20);
            lb_title2.Location = new Point(110, 0);
            lb_title2.TextAlign = ContentAlignment.MiddleCenter;
            lb_title2.BorderStyle = BorderStyle.FixedSingle;
            lb_title2.Text = "Values";
            panel2.Controls.Add(lb_title2);

            Label lb_title3 = new Label();
            lb_title3.Size = new Size(110, 20);
            lb_title3.Location = new Point(220, 0);
            lb_title3.TextAlign = ContentAlignment.MiddleCenter;
            lb_title3.BorderStyle = BorderStyle.FixedSingle;
            lb_title3.Text = "Bel/" + element.Text;
            panel2.Controls.Add(lb_title3);

            Label lb_title4 = new Label();
            lb_title4.Size = new Size(110, 20);
            lb_title4.Location = new Point(330, 0);
            lb_title4.TextAlign = ContentAlignment.MiddleCenter;
            lb_title4.BorderStyle = BorderStyle.FixedSingle;
            lb_title4.Text = "Pl/" + element.Text;
            panel2.Controls.Add(lb_title4);

            for (int i = 0; i < n; i++)
            {
                /*Thiet lap cac label chua ten cac tap hop*/
                lb_sets[i] = new Label();
                lb_sets[i].Size = new Size(110, 20);
                lb_sets[i].Location = new Point(0, i * 20 + 20);
                lb_sets[i].BorderStyle = BorderStyle.FixedSingle;
                //lb_sets[i].Text = "set " + (i + 1);


                /*Thiet lap cac textbox nhap gia tri*/
                tb_values[i] = new TextBox();
                tb_values[i].Size = new System.Drawing.Size(110, 20);
                tb_values[i].Location = new Point(110, i * 20 + 20);

                /*Thiet lap cac textbox chua Bel*/
                tb_Bel[i] = new TextBox();
                tb_Bel[i].Size = new System.Drawing.Size(110, 20);
                tb_Bel[i].Location = new Point(220, i * 20 + 20);
                tb_Bel[i].ReadOnly = true;
                tb_Bel[i].BackColor = Color.White;

                /*Thiet lap cac textbox chua Pl*/
                tb_Pl[i] = new TextBox();
                tb_Pl[i].Size = new System.Drawing.Size(110, 20);
                tb_Pl[i].Location = new Point(330, i * 20 + 20);
                tb_Pl[i].ReadOnly = true;
                tb_Pl[i].BackColor = Color.White;

                panel2.Controls.Add(lb_sets[i]);
                panel2.Controls.Add(tb_values[i]);

                panel2.Controls.Add(tb_Bel[i]);
                panel2.Controls.Add(tb_Pl[i]);
            }
        }

        private void GenSets()
        {
            try
            {
                GenSubsets g = new GenSubsets(element.next.Count);
                int[,] bit = g.getResult();

                for (int i = 0; i < g.row; i++)
                {
                    Set tmp = new Set();
                    for (int j = 0; j < g.n; j++)
                    {
                        if (bit[i, j] == 1)
                            tmp.Add(element.next[j]);
                    }
                    sets.Add(tmp);
                }
            }
            catch (Exception)
            {
            }
        }

        private void InitNamesOfSets()
        {
            try
            {
                for (int i = 0; i < n; i++)
                {
                    if (i < element.next.Count)
                    {
                        lb_sets[i].Text = "{" + (i+1) + "} " + element.next[i].Text;
                    }
                    else
                    {
                        string name = "{";
                        foreach (var item in sets[i])
                        {
                            name += (element.next.IndexOf(item) + 1) + ",";
                        }
                        name = name.Remove(name.Length - 1) + "}";
                        lb_sets[i].Text = name;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void updateBelPl()
        {
            bel.Clear();
            pl.Clear();
            try 
	        {	        
		        double sumValue = 0;
                for (int i = 0; i < n; i++)
			    {
			        sumValue += double.Parse(tb_values[i].Text);
			    }
                if (sumValue == 0)
                    throw new Exception();

                for (int i = 0; i < n; i++)
                {
                    m[i] = double.Parse(tb_values[i].Text) / sumValue;

                    Set Di = sets.ElementAt(i);
                    double tmp = 0;
                    for (int j = 0; j <= i; j++)
                    {
                        if(Di.Chua(sets.ElementAt(j)))
                        {
                            tmp += m[j];
                        }
                    }
                    tb_Bel[i].Text = tmp + "";
                    bel.Add(sets[i].Name,tmp);
                }

                for (int i = 0; i < n; i++)
                {
                    Set Di = sets.ElementAt(i);
                    double tmp = 0;

                    for (int j = 0; j < n; j++)
                    {
                        if(Di.GiaoKhacRong(sets.ElementAt(j)))
                        tmp += m[j];  
                    }

                    tb_Pl[i].Text = tmp + "";
                    pl.Add(sets[i].Name, tmp);
                }
	        }
	        catch (Exception)
	        {
                for (int i = 0; i < n; i++)
			    {
                    tb_Bel[i].Text = "???";
                    tb_Pl[i].Text = "???";
                    bel.Clear();
                    pl.Clear();
			    }
	        }

        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                element.value.Clear();
                for (int i = 0; i < n; i++)
                {
                    element.value.Add(double.Parse(tb_values[i].Text));
                }

                this.updateBelPl();
                
                element.Bel_next = bel;
                element.Pl_next = pl;

                element.entered = true;
                this.Dispose();
            }
            catch (Exception)
            {                
                MessageBox.Show("Đã xảy ra lỗi, mời nhập lại!");
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            try
            {
                element.value.Clear();
                for (int i = 0; i < n; i++)
                {
                    element.value.Add(double.Parse(tb_values[i].Text));
                }

                this.updateBelPl();

                element.Bel_next = bel;
                element.Pl_next = pl;

                element.entered = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi, mời nhập lại!");
            }
        }
    }
}
