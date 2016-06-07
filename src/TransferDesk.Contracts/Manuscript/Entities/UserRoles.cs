using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class UserRoles
    {
        [Key]
        public int ID { get; set; }
        public int RollID { get; set; }
        public int DefaultRollID { get; set; }
        public string UserID { get; set; }
        public bool IsActive { get; set; }
        public int? Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
    }
}
