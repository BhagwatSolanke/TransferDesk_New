
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class OtherAuthor
    {
        [Key]
        public int? ID { get; set; }
        public string MSID { get; set; }
        public string AuthorName { get; set; }
        public string Affillation { get; set; }
        public int Status { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }
        public int? ManuscriptID { get; set; }

        public OtherAuthor()
        {

        }
    }
}