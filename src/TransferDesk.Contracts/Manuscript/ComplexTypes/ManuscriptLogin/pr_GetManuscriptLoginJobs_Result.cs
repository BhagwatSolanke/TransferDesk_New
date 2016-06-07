using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptLogin
{   
    public class pr_GetManuscriptLoginJobs_Result
    {
        public int CrestId { get; set; }
        public string ServiceType { get; set; }
        public string JournalTitle { get; set; }
        public string MSID { get; set; }
        public string ArticleTypeName { get; set; }
        public string SectionName { get; set; }
        public string ManuscriptFilePath { get; set; }
        public string Link { get; set; }
        public string ArticleTitle { get; set; }
        public string SpecialInstruction { get; set; }     
        public string Associate { get; set; }
        public DateTime InitialSubmissionDate { get; set; }
    }


    public class pr_GetManuscriptLoginExportJobs_Result
    {
        public int CrestId { get; set; }
        public string ServiceType { get; set; }
        public string JournalTitle { get; set; }
        public string MSID { get; set; }
        public string ArticleTypeName { get; set; }
        public string SectionName { get; set; }
        public string Link { get; set; }
        public string ArticleTitle { get; set; }
        public string SpecialInstruction { get; set; }
        public string Associate { get; set; }
        public string InitialSubmissionDate { get; set; }
    }


}
