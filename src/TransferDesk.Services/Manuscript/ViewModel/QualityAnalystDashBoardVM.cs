using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AssociateDashBoard;
using TransferDesk.Contracts.Manuscript.ComplexTypes.QualityAnalystDashBoard;

namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class QualityAnalystDashBoardVM
    {
        public IEnumerable<pr_GetSpecificQualityAnalystJobs_Result> specificQualityAnalystdetails { get; set; }
    }
}
