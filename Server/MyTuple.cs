using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class MyTuple
    {
        public int Item1 { get; set; }
        public int Item2 { get; set; }
        public MyTuple(int item1, int item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}
