using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptLogin;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class ManuscriptLoginVM
    {
        public int CrestId { get; set; }
        public List<Journal> Journal { get; set; }
        public List<ArticleType> ArticleType { get; set; }
        public List<Section> Section { get; set; }
        public List<StatusMaster> TaskList { get; set; }
        public List<StatusMaster> ServiceType { get; set; }
        public List<pr_GetManuscriptLoginJobs_Result> ManuscriptLoginedJobs { get; set; }
        [Required(ErrorMessage = "MSID")]
        public string MSID { get; set; }

        [Required(ErrorMessage = "Service Type")]
        public int ServiceTypeID { get; set; }

        [Required(ErrorMessage = "Journal Title")]
        public int JournalID { get; set; }
        
        [Required(ErrorMessage = "Article Type")]
        public int ArticleTypeID { get; set; }
        
        public int? SectionID { get; set; }
        
        [Required(ErrorMessage = "Article Title")]
        public string ArticleTitle { get; set; }
        
        public string SpecialInstruction { get; set; }
        
        [Required(ErrorMessage = "Initial Submission Date")]
        public DateTime InitialSubmissionDate { get; set; }

        [Required(ErrorMessage = "Received Date")]
        public DateTime? ReceivedDate { get; set; }

        //[Required(ErrorMessage = "Associate Name")]
        public string Associate { get; set; }

        //[Required(ErrorMessage = "Upload Manuscript")]
        public string ManuscriptFilePath { get; set; }

        public string EmployeeName { get; set; }

        public string userID { get; set; }

        public bool IsRevision { get; set; }

        public int? TaskID { get; set; }

    }
}
