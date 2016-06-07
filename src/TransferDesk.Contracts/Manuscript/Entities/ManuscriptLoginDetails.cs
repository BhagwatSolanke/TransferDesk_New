using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class ManuscriptLoginDetails
    {
        [Key]
        public int Id { get; set; }
        public int CrestId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? FetchedDate { get; set; }
        public DateTime? SubmitedDate { get; set; }
        public int JobStatusId { get; set; }
        public int ServiceTypeStatusId { get; set; }
        public int RoleId { get; set; }
        public int? UserRoleId { get; set; }
        public int? JobProcessStatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; } 
    }
}
