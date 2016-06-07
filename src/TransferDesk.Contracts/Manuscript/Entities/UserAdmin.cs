using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class UserAdmin
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        public bool IsAdmin{ get; set; }
        public int? Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
    }
}
