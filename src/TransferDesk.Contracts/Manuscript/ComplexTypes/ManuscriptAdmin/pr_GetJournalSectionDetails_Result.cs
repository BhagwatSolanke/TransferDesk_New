using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptAdmin
{
    public partial class pr_GetJournalSectionDetails_Result
    {
        public int ID { get; set; }
        public string JrTitle { get; set; }
        public int JrID { get; set; }
        public string SectionValue { get; set; }
        public bool IsActive { get; set; }
        public int SectionTypeID { get; set; }

    }
}
