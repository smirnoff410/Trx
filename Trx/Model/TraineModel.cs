using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trx.Model
{
    public class TraineModel
    {
        public int Id { get; set; }
        public string type { get; set; }
        public int price { get; set; }
        public int subscription { get; set; }
        public int count_raine { get; set; }
        public decimal validity { get; set; }
    }
}
