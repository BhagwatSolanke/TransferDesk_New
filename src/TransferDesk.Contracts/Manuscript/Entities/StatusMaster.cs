using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
   public class StatusMaster
    {
        [Key]
        public int ID { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
    }
}
