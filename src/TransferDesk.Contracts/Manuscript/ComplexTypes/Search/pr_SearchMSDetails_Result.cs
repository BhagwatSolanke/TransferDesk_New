
using System;
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.Manuscript.ComplexTypes.Search
{
    public class pr_SearchMSDetails_Result
    {
        public int ID { get; set; }
        public string MSID { get; set; }
        //public int JournalID { get; set; }
        public string JournalTitle { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleTypeName { get; set; }
        public string SectionName { get; set; }
        
    }
}
