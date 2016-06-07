using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class MSReviewerErrorCategory
    {
        [Key]
        public int ID { get; set; }
        public string MSID { get; set; }
        public int MSReviewersSuggestionID { get; set; }
        public int ErrorCategoryID { get; set; }
        public bool? Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
        public bool? IsUncheckedByUser { get; set; }
    }
}
