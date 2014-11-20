using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHP
{
    public class Element : TextBox
    {
        static int quantity = 0; //tong so luong cac element
        public string id { get; set; }
        public List<Element> next;
        public List<Element> prev;
        public List<double> value;
        
        public Hashtable Bel_next;
        public Hashtable Pl_next;

        public bool entered = false;   //da duoc nhap hay chua

        public Element()
            : base()
        {
            next = new List<Element>();
            prev = new List<Element>();
            value = new List<double>();
            
            quantity++;
            id = "" + quantity;
        }
    }
}
