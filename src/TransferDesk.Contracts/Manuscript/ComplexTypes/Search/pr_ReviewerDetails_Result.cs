using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.ComplexTypes.Search
{
    public class pr_ReviewerDetails_Result
    {
        public int ID { get; set; }
        public string MSID { get; set; }
        public string JournalTitle { get; set; }
        public string ArticleTitle { get; set; }
    }
}
