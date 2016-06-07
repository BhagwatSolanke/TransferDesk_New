using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class ManuscriptErrorCategory
    {
        [Key]
        public int ID { get; set; }
        public string MSID { get; set; }
        public int ErrorCategoryID { get; set; }
        public int? Status { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }
        public int? ManuscriptID { get; set; }
        public bool IsUncheckedByUser { get; set; }
        public ManuscriptErrorCategory()
        {

        }
    }
}
