
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class Journal
    {   
        [Key]
        public int ID { get; set; }
        public string JournalTitle { get; set; }
        public int Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
        public bool IsActive { get; set; }
        public string Link { get; set; }
    }
}