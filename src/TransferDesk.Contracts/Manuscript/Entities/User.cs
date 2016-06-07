
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public int EmpID { get; set; }
        public string EmpUserID { get; set; }
        public string EmpName { get; set; }   
        public string EmpPhone { get; set; }
        public string Email{ get; set; }
        public bool IsActive { get; set; }   
        public int? Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
    }
}