using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEHEMO.DTO
{
    public class DTO_TEST
    {
        public Guid ID { get; set; }
        public string URL { get; set; }
        public bool? STATUS { get; set; }
        public string STATUS_DESCRIPTION { get; set; }
        public DateTime? DATE { get; set; }
        public DateTime CREATE_DATE { get; set; }

    }
}
