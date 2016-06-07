//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TransferDesk.DAL.Manuscript.testEdmx
{
    using System;
    using System.Collections.Generic;
    
    public partial class Manuscript
    {
        public int ID { get; set; }
        public int JournalID { get; set; }
        public int MSID { get; set; }
        public int ArticleTypeID { get; set; }
        public Nullable<int> SectionID { get; set; }
        public string ArticleTitle { get; set; }
        public string CorrespondingEditor { get; set; }
        public string AssignedEditor { get; set; }
        public System.DateTime StartDate { get; set; }
        public int RoleID { get; set; }
        public int UserMasterID { get; set; }
        public int Crosscheck_iThenticateResultID { get; set; }
        public decimal Highest_iThenticateFromSingleSrc { get; set; }
        public int English_Lang_QualityID { get; set; }
        public string Conclusion { get; set; }
        public int Ethics_ComplianceID { get; set; }
        public string TransferFrom { get; set; }
        public string ReviewerComments { get; set; }
        public string Abstarct { get; set; }
        public System.DateTime InitialSubmissionDate { get; set; }
        public Nullable<System.DateTime> AssociateTAT { get; set; }
        public Nullable<bool> QualityCheck { get; set; }
        public Nullable<System.DateTime> QualityStartCheckDate { get; set; }
        public Nullable<int> ErrorCategoryID { get; set; }
        public Nullable<System.DateTime> QualityEndDate { get; set; }
        public Nullable<System.DateTime> FinalSubmitDate { get; set; }
        public Nullable<System.DateTime> QualityTAT { get; set; }
        public int Status { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }
        public string CorrespondingAuthor { get; set; }
        public string CorrespondingAuthorEmail { get; set; }
        public string CorrespondingAuthorAff { get; set; }
        public Nullable<decimal> iThenticatePercentage { get; set; }
        public string OverallAnalysis { get; set; }
        public Nullable<bool> HasTransferReport { get; set; }
    }
}