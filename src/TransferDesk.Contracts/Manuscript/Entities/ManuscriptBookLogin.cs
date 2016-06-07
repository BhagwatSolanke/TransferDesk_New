using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class ManuscriptBookLogin
    {
        public int ID { get; set; }
        public string CrestID { get; set; }
        public int BookMasterID { get; set; }
        public int ServiceTypeID { get; set; }
        public int? StatusMasterTaskID { get; set; }
        public string ChapterNumber { get; set; }
        public string ChapterTitle { get; set; }
        public int PageCount { get; set; }
        public System.DateTime ReceivedDate { get; set; }
        public string RequesterName { get; set; }
        public string SpecialInstruction { get; set; }
        public int? Revision { get; set; }
        public int? RevisionParentID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifidedDate { get; set; }
        public string ModifidedBy { get; set; }
        public int ManuscriptStatusID { get; set; }
    }
}
