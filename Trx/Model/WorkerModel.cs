using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trx.Model
{
    public class WorkerModel
    {
        public int Id { get; set; }
        public string first_name {get;set;}
        public string second_name { get; set; }
        public string last_name { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public int id_role { get; set; }
    }
}
