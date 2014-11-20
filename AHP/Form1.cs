using System;
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
    public partial class Form1 : Form
    {
        const int LABEL_WIDTH = 150;
        const int LABEL_HEIGHT = 24;

        Label[] lb_level = new Label[20];
        Element el_seleted = null;
        List<Element> list = new List<Element>();

        Element goal = null;
        List<Element> alternatives = new List<Element>();

        bool multigoal = false;  //co nhieu hon 1 goal
        Graphics graph;
        
        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            graph = pn_Main.CreateGraphics();
            pn_Main.Paint += update_lines;

            for (int i = 0; i < 20; i++)
            {
                lb_level[i] = new Label();
                lb_level[i].Size = new Size(LABEL_WIDTH,LABEL_HEIGHT);
                lb_level[i].BorderStyle = BorderStyle.FixedSingle;
                
                lb_level[i].Location = new Point(i * LABEL_WIDTH, 0);
                pn_Main.Controls.Add(lb_level[i]);
            }

            pn_Main.AutoScroll = true;            
            
            Label corner = new Label();
            corner.Location = new Point(1000, 500);
            pn_Main.Controls.Add(corner);
            
        }

        private void pn_Main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            update_color();

            Element newElement = new Element();
            
            /* Xac dinh toa do x, y*/
            int col = 1;
            while (lb_level[col].Location.X < e.X)
            {
                col++;
            }
            col--;
            int x = lb_level[col].Location.X + 20;
            int y = ((e.Y - lb_level[col].Location.Y) / (LABEL_HEIGHT + 10)) * (LABEL_HEIGHT + 10) + lb_level[col].Location.Y;

            newElement.Text = "Element" + (list.Count + 1);
            newElement.Size = new Size(110, 24);
            newElement.Location = new Point(x, y);

            newElement.LostFocus += tb_Element_LostFocus;            
            newElement.MouseDown += tb_Element_MouseDown;
            newElement.DoubleClick += tb_Element_DoubleClick;
            newElement.KeyPress += tb_Element_KeyPress;

            newElement.ContextMenu = new ContextMenu();
            pn_Main.Controls.Add(newElement);
            newElement.Focus();
            newElement.SelectAll();

            el_seleted = newElement;
            list.Add(newElement);
        }

        private void tb_Element_KeyPress(object sender, KeyPressEventArgs e)
        {
            Element el = (Element)sender;
            if (e.KeyChar == 13)
            {
                el.ReadOnly = true;
                update_color();
            }
        }

        private void tb_Element_DoubleClick(object sender, EventArgs e)
        {
            (new Form2((Element)sender)).ShowDialog();
        }

        private void tb_Element_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    el_seleted = (Element)sender;
                    update_color();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    Element dist = (Element)sender;

                    if (el_seleted != null)
                    {                        
                        if (el_seleted.Location.X > dist.Location.X)
                        {                            
                            if (!dist.next.Contains(el_seleted))
                            {
                                dist.next.Add(el_seleted);
                                el_seleted.prev.Add(dist);
                            }
                        }

                        else if (el_seleted.Location.X < dist.Location.X)
                        {
                            
                            if (!el_seleted.next.Contains(dist))
                            {
                                el_seleted.next.Add(dist);
                                dist.prev.Add(el_seleted);
                            }
                        }

                        update_levels();
                        update_lines(pn_Main, null);
                        update_color();
                    }
                }
            }
            catch (Exception) { };
        }

        private void tb_Element_LostFocus(object sender, EventArgs e)
        {
            ((TextBox)sender).ReadOnly = true;
        }

        private void analyseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var element in list)
            {
                if (element.next.Count == 0 && element.prev.Count == 0)
                    continue;
                if (!element.entered && !alternatives.Contains(element))
                {
                    MessageBox.Show("Chua nhap du so lieu");
                    return;
                }
            }

            if (multigoal)
            {
                MessageBox.Show("Co nhieu hon mot goal");
                return;
            }

            try
            {
                (new Form3(goal, alternatives)).ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Bieu do khong hop le");
            }
        }

        private void update_lines(object sender, PaintEventArgs e)
        {
            graph.Clear(Color.White);
            Pen myPen = new Pen(Color.Black, 1);
            Point p1, p2;
            foreach (var el1 in list)
            {
                foreach (var el2 in el1.next)
                {
                    p1 = new Point(el1.Location.X + 110, el1.Location.Y + LABEL_HEIGHT / 2);
                    p2 = new Point(el2.Location.X, el2.Location.Y + LABEL_HEIGHT / 2);
                    graph.DrawLine(myPen, p1, p2);
                }
            }
        }
        
        private void update_color()
        {
            foreach (var item in list)
            {
                if (item == el_seleted)
                {
                    item.BackColor = Color.Blue;
                    item.ForeColor = Color.White;
                }
                else if (alternatives.Contains(item))
                {
                    item.BackColor = Color.Yellow;
                    item.ForeColor = Color.Black;
                }
                else
                {
                    item.BackColor = Color.Cyan;
                    item.ForeColor = Color.Black;
                }
            }
        }

        private void update_levels()
        {
            goal = null;
            multigoal = false;
            alternatives.Clear();

            foreach (var label in lb_level)
            {
                label.Text = "";
            }

            foreach (var element in list)
            {
                if (element.prev.Count == 0 && element.next.Count > 0)
                {
                    if (goal == null)
                        goal = element;
                    else
                    {
                        multigoal = true;
                        if (goal.Location.X > element.Location.X)
                            goal = element;
                    }
                }
                else if (element.next.Count == 0 && element.prev.Count > 0)
                    alternatives.Add(element);
            }

            if (goal != null)
            {
                int i = goal.Location.X / LABEL_WIDTH;
                lb_level[i].Text = "Goal";

                Element tmp = goal.next[0];
                i = 1;
                while (!alternatives.Contains(tmp))
                {
                    lb_level[tmp.Location.X / LABEL_WIDTH].Text = "Criteria " + i;
                    tmp = tmp.next[0];
                    i++;
                }

                foreach (var item in alternatives)
                {
                    i = item.Location.X / LABEL_WIDTH;
                    if (lb_level[i].Text == "")
                        lb_level[i].Text = "Alternatives";
                }                
            }
        }

        private void pn_Main_MouseClick(object sender, MouseEventArgs e)
        {
            el_seleted = null;
            update_color();
        }

        private void mni_EditElementName_Click(object sender, EventArgs e)
        {
            if (el_seleted != null)
            {
                el_seleted.ReadOnly = false;
                el_seleted.BackColor = Color.White;
                el_seleted.ForeColor = Color.Black;
                el_seleted.SelectAll();
            }
        }

        private void mni_DeleteElement_Click(object sender, EventArgs e)
        {
            if (el_seleted != null)
            {
                foreach (var item in el_seleted.prev)
                {
                    item.next.Remove(el_seleted);
                }

                foreach (var item in el_seleted.next)
                {
                    item.prev.Remove(el_seleted);
                }

                pn_Main.Controls.Remove(el_seleted);

                update_levels();
                update_lines(pn_Main, null);
                update_color();
            }
        }
    }
}
