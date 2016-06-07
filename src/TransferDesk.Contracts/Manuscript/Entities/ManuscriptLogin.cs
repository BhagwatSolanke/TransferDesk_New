using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class ManuscriptLogin
    {
        [Key]
        public int CrestId { get; set; }
        public int JournalId { get; set; }
        public int ArticleTypeId { get; set; }
        public int? SectionId { get; set; }
        public string MSID { get; set; }
        public string ArticleTitle { get; set; }
        public DateTime InitialSubmissionDate { get; set; }
        public int ServiceTypeStatusId { get; set; }
        public int ManuscriptStatusId { get; set; }
        public string ManuscriptFilePath { get; set; }
        public int? Revision { get; set; }
        public string SpecialInstruction { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReportSentDate { get; set; }
        public int? InvoiceID { get; set; }
        public int? DeliveryAdviceID { get; set; }
        public DateTime? BackupDateTime { get; set; }
        public string BackupPath { get; set; }
        public int PriorityStatusId { get; set; }
        public int RevisionParentId { get; set; }
        public string CreatedUserID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int? TaskID { get; set; }

    }
}
