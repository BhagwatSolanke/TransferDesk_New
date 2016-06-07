
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class ErrorCategory
    {
        [Key]
        public int ID { get; set; }
        public string ErrorCategoryName { get; set; }
        public bool? Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
        public string ManuscriptType { get; set; }
        public ErrorCategory()
        {

        }
    }


}
