using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
   public class ReviewerMaster
    {
        [Key]
        public int ID { get; set; }
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string ReviewerName { get; set; }
        public int? CityId { get; set; }
        public string StreetName { get; set; }
        public int? InstituteID { get; set; }
        public int? DeptID { get; set; }
        public int? NoOfPublication { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
    
    }
}
