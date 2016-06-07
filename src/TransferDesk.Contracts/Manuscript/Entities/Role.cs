
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class Role
    {
        [Key]
        public int ID { get; set; }
        public string RoleName { get; set; }
        public int? Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
    }
}
