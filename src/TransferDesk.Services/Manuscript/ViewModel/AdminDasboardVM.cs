using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AdminDashBoard;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Services.Manuscript.ViewModel
{
  public  class AdminDasboardVM
    {

        public int CrestIdVM;
        //public string Jobtype;
        public string ServiceTypeVM; 
        //public string MSID;
        //public string JournalName;
        //public int? PageCount;
        //public string AssociateName;
        public string RoleVM;
        public string JobProcessingStatusVM;
        public string AssociateNameVM;
      public string StatusVm;
        //public string Task;
        //public int? RevisionName;
        //public int? GroupNo;
        //public DateTime RecievedDate;
        //public DateTime LoggedinDate;
        //public DateTime FetchDate;
        //public int? Age;
        //public int? HandlingTime;

        public IEnumerable<pr_AdminDashBoardGetAllOpenJobs_Result> jobsdetails { get; set; }

    }
}
