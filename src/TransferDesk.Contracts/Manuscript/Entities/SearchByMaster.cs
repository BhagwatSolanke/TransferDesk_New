
using System.ComponentModel.DataAnnotations;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public partial class SearchByMaster
    {
        [Key]
        public int ID { get; set; }
        public string SearchByName { get; set; }
        public int? Status { get; set; }
        public System.DateTime? Modify_dttm { get; set; }
        public string ManuscriptType { get; set; }
    }
}
