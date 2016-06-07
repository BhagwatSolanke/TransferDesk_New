using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class ReviewerErrorCategory
    {
         [Key]
        public int ID { get; set; }
        public string ErrorCategoryName { get; set; }
        public bool? Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
        public int? ErrorWeightage { get; set; }
    }
}
