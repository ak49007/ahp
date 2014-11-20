using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace AHP
{
    public class Set : List<Element>
    {
        public string Name
        {
            get
            {
                string str = "";
                foreach (var item in this)
                {
                    str += (item.id + " ");
                }
                return str;
            }
        }

        public bool Chua(Set s)
        {
            foreach (var item in s)
            {
                if (!this.Contains(item))
                    return false;
            }
            return true;
        }

        public bool GiaoKhacRong(Set s)
        {
            foreach (var item in s)
            {
                if(this.Contains(item))
                    return true;
            }
            return false;
        }
    }
}
