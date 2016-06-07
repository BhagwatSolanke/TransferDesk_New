
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class Manuscript
    {
        [Key]
        public int ID { get; set; }
        public int? JournalID { get; set; }
        public string MSID { get; set; }//equivalent to big int in sql server 2012 version types
        public int? ArticleTypeID { get; set; }
        public int? SectionID { get; set; }
        
        public string ArticleTitle { get; set; }
        public string CorrespondingEditor { get; set; }
        public string AssignedEditor { get; set; }
        public System.DateTime StartDate { get; set; }
        public int? RoleID { get; set; }
        public string UserID { get; set; }
        public int? Crosscheck_iThenticateResultID { get; set; }
        public decimal? Highest_iThenticateFromSingleSrc { get; set; }
        public int? English_Lang_QualityID { get; set; }
        public string Conclusion { get; set; }
        public int? Ethics_ComplianceID { get; set; }
        public string TransferFrom { get; set; }
        public string ReviewerComments { get; set; }
        public string Abstarct { get; set; }
        public System.DateTime InitialSubmissionDate { get; set; }
        public System.DateTime? AssociateTAT { get; set; }
        public bool? QualityCheck { get; set; }
        public System.DateTime? QualityStartCheckDate { get; set; }
        public System.DateTime? QualityEndDate { get; set; }
        public System.DateTime? FinalSubmitDate { get; set; }
        public System.DateTime? QualityTAT { get; set; }
        public int Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
        public string CorrespondingAuthor { get; set; }
        public string CorrespondingAuthorEmail { get; set; }
        public string CorrespondingAuthorAff { get; set; }
        public decimal? iThenticatePercentage { get; set; }
        public string OverallAnalysis { get; set; }
        public bool? HasTransferReport { get; set; }
        public string Accurate { get; set; }
        public string ErrorDescription { get; set; }
        public string QualityUserID { get; set; }
        public int? OverallAnalysisID { get; set; }
        public bool? IsAccurate { get; set; }
        public string Comments_English_Lang_Quality { get; set; }
        public string Comments_Ethics_Compliance { get; set; }
        public string Comments_Crosscheck_iThenticateResult { get; set; }
        public int? ParentManuscriptID { get; set; }
        public System.DateTime? RevisedDate { get; set; }

        public string Comments_OverallAnalysis { get; set; }

        public bool? IsAssociateFinalSubmit { get; set; }

        public bool? IsQualityFinalSubmit { get; set; }

        public string HandlingEditor { get; set; }

        public int? JournalStatusID { get; set; }

        public Manuscript() { 
        }
    }
}