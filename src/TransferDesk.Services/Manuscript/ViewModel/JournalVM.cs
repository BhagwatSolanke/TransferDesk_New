using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Services.Manuscript.ViewModel
{
   public  class JournalVM
    {

        public int ID { get; set; }
        [Required(ErrorMessage = "Journal Title is required")]
        [RegularExpression(@"([^\~\`\!\@\#\$\%\^\&\*\(\)_\-\=\+\:\;\?\/\>\<\.\,]+[^\n]+){1,}", ErrorMessage = "Journal Title is not valid")]
        public string JournalTitle { get; set; }
        [Url]
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<Journal> Journals { get; set; }

    }
}
