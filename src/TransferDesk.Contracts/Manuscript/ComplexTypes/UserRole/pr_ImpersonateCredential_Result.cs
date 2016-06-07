using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace TransferDesk.Contracts.Manuscript.ComplexTypes.UserRole
{
    public class pr_ImpersonateCredential_Result
    {
        //public int ID { get; set; }
        public string ServerUserName { get; set; }
        public string Domain { get; set; }
        public string Password { get; set; }
    }
}
