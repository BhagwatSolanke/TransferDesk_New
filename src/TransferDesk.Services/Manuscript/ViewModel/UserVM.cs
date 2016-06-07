using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class UserVM
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "User id is required")]
        public string EmpUserID { get; set; }

        [Required(ErrorMessage = "Employee name is required")]
        public string EmpName { get; set; }

        [Required(ErrorMessage = "Employee id is required")]
        public int? EmpID { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        public IEnumerable<User> users { get; set; }

        public bool IsActive { get; set; }
    }
}
