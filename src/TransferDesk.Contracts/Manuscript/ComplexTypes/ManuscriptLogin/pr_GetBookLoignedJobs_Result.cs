using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptLogin
{
    public class pr_GetBookLoignedJobs_Result
    {
        public int ID { get; set; }
        public string BookTitle{ get; set; }
        public string CrestID{ get; set; }
        public string ChapterNumber{ get; set; }
        public string FTPLink{ get; set; }
        public string GPUInformation{ get; set; }
        public string ChapterTitle{ get; set; }
        public int PageCount{ get; set; }
        public DateTime ReceivedDate{ get; set; }
        public string RequesterName{ get; set; }
        public string Associate{ get; set; }
        public string SpecialInstruction{ get; set; }
        public string ServiceType{ get; set; }
        public string Task{ get; set; }
    }

    public class pr_GetManuscriptBookLoginExportJobs_Result
    {
        public string BookTitle { get; set; }
        public string CrestId { get; set; }
        public string ChapterNumber { get; set; }
        public string FTPLink { get; set; }
        public string GPUInformation { get; set; }
        public string ChapterTitle { get; set; }
        public int PageCount { get; set; }
        public string ReceivedDate { get; set; }
        public string RequesterName { get; set; }
        public string Associate { get; set; }
        public string SpecialInstruction { get; set; }
        public string ServiceType { get; set; }
        public string Task { get; set; }
    }
}
