using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Application
    {
        public Guid Uid { get; set; }
        public string Name { get; set; }
        public string Cmd { get; set; }

    }
    public class Schedule {
        public string Cron { get; set; }
        public string Paras { get; set; }
        public int Enable { get; set; }
    }
}
