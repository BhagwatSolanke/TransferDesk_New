using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class MSReviewerLink
    {
        //[Key]
        //public int ID { get; set; }
        //public int ReviewerMasterID { get; set; }
        //public string Link { get; set; }
        //public bool? IsActive { get; set; }
        //public string CreatedBy { get; set; }
        //public System.DateTime? CreatedDate { get; set; }
        //public string ModifiedBy { get; set; }
        //public System.DateTime? ModifiedDate { get; set; }

        [Key]
        public int ID { get; set; }
        public int MSReviewersSuggestionInfoID { get; set; }
        public String Link { get; set; }

    }
}
