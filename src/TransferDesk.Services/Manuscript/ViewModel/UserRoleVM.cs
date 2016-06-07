using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.UserRole;
using TransferDesk.Contracts.Manuscript.Entities;
namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class UserRoleVM
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "User id is required")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int RollID { get; set; }

        public IEnumerable<Role> Role { get; set; }

        public IEnumerable<pr_GetUserRoleDetails_Result> userRoles { get; set; }

        public bool IsActive { get; set; }

        public string EmployeeName { get; set; }
    }
}
