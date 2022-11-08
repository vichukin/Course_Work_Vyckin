using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Work_Vyckin
{
    public class StepInfo
    {
        public MyTuple From { get; set; }
        public MyTuple To { get; set; }
        public MyTuple Fight { get; set; } = new MyTuple(50,50) ;
        //public bool Queen;
        public StepInfo() { }
    }
}
