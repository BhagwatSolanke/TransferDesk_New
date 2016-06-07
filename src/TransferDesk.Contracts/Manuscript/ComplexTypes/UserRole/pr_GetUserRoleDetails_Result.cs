using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.ComplexTypes.UserRole
{
    public partial class pr_GetUserRoleDetails_Result
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public bool IsActive { get; set; }
        public int RollID { get; set; }
        public string RoleName { get; set; }
    }
}
