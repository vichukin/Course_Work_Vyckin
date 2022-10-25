using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class StepInfo
    {
        public (int,int) From { get; set; }
        public (int,int) To { get; set; }
        public (int,int) Fight { get; set; } = (50,50);
    }
}
