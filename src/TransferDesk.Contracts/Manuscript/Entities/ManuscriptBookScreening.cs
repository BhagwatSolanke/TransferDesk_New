using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class ManuscriptBookScreening
    {
        public int ID { get; set; }
        public int BookLoginID { get; set; }
        public int? OverallAnalysisID { get; set; }
        public int? Crosscheck_iThenticateResultID { get; set; }
        public int? Highest_iThenticateFromSingleSrc { get; set; }
        public int? English_Lang_QualityID { get; set; }
        public int? Ethics_ComplianceID { get; set; }
        public bool? QualityCheck { get; set; }
        public string CorrespondingAuthor { get; set; }
        public string CorrespondingAuthorEmail { get; set; }
        public string CorrespondingAuthorAff { get; set; }
        public int? iThenticatePercentage { get; set; }
        public bool? Accurate { get; set; }
        public string ErrorDescription { get; set; }
        public string Comments_English_Lang_Quality { get; set; }
        public string Comments_Ethics_Compliance { get; set; }
        public string Comments_Crosscheck_iThenticateResult { get; set; }
        public string Comments_OverallAnalysis { get; set; }
        public bool? IsAssociateFinalSubmit { get; set; }
        public bool? IsQualityFinalSubmit { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifidedDate { get; set; }
        public string ModifidedBy { get; set; }
        public DateTime? QualityStartCheckDate { get; set; }
    }
}
