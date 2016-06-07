using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
   public class Employee
    {
        public string EmpID { get; set; }
        public string username { get; set; }
        public string EmpName { get; set; }
        public string alternateEmail { get; set; }
    }
}
