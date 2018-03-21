using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trx.Model
{
    public class ScheduleModel
    {
        public int Id { get; set; }
        public string worker { get; set; }
        public string traine { get; set; }
        public decimal date_start { get; set; }
        public decimal date_end { get; set; }
    }
}
