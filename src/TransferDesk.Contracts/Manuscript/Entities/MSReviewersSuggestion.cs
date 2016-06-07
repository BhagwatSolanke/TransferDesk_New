using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class MSReviewersSuggestion
    {
        [Key]
        public int ID { get; set; }
        public string MSID { get; set; }
        public int SMTaskID { get; set; }
        public int RoleID { get; set; }
        //public int ReviewerMasterID { get; set; }
        public string AnalystUserID { get; set; }
        public string QualityUserID { get; set; }
        public System.DateTime? AnalystSubmissionDate { get; set; }
        public System.DateTime? QualitySubmissionDate { get; set; }
        public bool? IsAssociateFinalSubmit { get; set; }
        public bool? IsQualityFinalSubmit { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime? ModifyDate { get; set; }
        public System.DateTime? StartDate { get; set; }
        public System.DateTime? QualityStartDate { get; set; }
        public bool? QualityCheck { get; set; }
        public System.DateTime? AssociateTAT { get; set; }
        public System.DateTime? QualityTAT { get; set; }
        public bool? IsAccurate { get; set; }
        public string ErrorDescription { get; set; }
        public string JobType { get; set; }
        public string ArticleTitle { get; set; }
        public int? JournalID { get; set; }
    }
}
