using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptAdmin;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Services.Manuscript.ViewModel
{
   public class JournalSectionTypeVM
    {
        public int ID { get; set; }
        public int JrID { get; set; }
        public string JournalTitleName { get; set; }
        public int SectionTypeID { get; set; }
        [Required(ErrorMessage = "Section Name is required")]
        public string SectionName { get; set; }

        [Required(ErrorMessage = "Journal Title is required")]
        public int JournalID { get; set; }

        public IEnumerable<Journal> Journals { get; set; }

        public IEnumerable<Section> SectionTypes { get; set; }

        public bool IsActive { get; set; }
        public IEnumerable<pr_GetJournalSectionDetails_Result> sectiondetails { get; set; }
    }
}
