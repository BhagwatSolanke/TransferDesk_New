using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Entities;
using System.ComponentModel.DataAnnotations;
using TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptAdmin;
namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class JournalArticleTypeVM
    {

        public int ID { get; set; }
        public int JrID { get; set; }
        public string JournalTitleName { get; set; }
        public int ArticleTypeID { get; set; }

        [Required(ErrorMessage = "Article Type is required")]
        public string ArticleTypeName { get; set; }

        [Required(ErrorMessage = "Journal Title is required")]
        public int JournalID { get; set; }

        public IEnumerable<Journal> Journals { get; set; }

        public IEnumerable<ArticleType> ArticleTypes { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<pr_GetJournalArticleDetails_Result> details { get; set; }
    }
}
