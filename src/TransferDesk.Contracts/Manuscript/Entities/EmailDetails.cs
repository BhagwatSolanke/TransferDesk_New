using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class EmailDetails
    {
        public int ID { get; set; }
        public string MSID { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public bool? Status { get; set; }
        public System.DateTime? SendDateTime { get; set; }
    }
}
