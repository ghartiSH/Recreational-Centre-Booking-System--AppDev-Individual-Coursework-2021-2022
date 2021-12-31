using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    public class CheckedoutVisitors
    {
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string category { get; set; }
        public DateTime inDateTime { get; set; }
        public int totalVisitors { get; set; }
        public int totalChildren { get; set; }
        public DateTime outDateTime { get; set; }
        public int paid { get; set; }
    }
}
