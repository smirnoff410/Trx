using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trx.Model
{
    public class UserTraineModel
    {
        public int Id { get; set; }
        public int id_user { get; set; }
        public string traine_type { get; set; }
        public int count_traine { get; set; }
        public decimal date_start { get; set; }
        public decimal date_finish { get; set; }
        public int price { get; set; }
        public int sale { get; set; }
    }
}
