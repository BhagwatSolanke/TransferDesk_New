
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class ArticleType
    {
        [Key]
        public int ID { get; set; }
        public string ArticleTypeName { get; set; }
        public int? Status { get; set; }
        public System.DateTime? ModifiedDateTime { get; set; }
        public bool IsActive { get; set; }
    }
}