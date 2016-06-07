using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class BookMaster
    {
        [Key]
        public int ID { get; set; }
        public string BookTitle { get; set; }
        public string GPUInformation { get; set; }
        public string FTPLink { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
