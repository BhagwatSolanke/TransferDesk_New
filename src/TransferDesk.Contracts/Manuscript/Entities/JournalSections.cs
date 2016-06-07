
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class JournalSections
    {
        [Key]
        public int ID { get; set; }
        public int JournalID { get; set; }
        public int SectionID { get; set; }
        public int? Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
