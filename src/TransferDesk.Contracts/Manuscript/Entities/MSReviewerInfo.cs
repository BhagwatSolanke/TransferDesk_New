using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class MSReviewersSuggestionInfo
    {
        [Key]
        public int ID { get; set; }
        public int MSReviewersSuggestionID { get; set; }
        public int ReviewerMasterID { get; set; }
        public int? InstitutionID { get; set; }
        public int? DeptID { get; set; }
        public string StreetName { get; set; }
        public int? CityID { get; set; }
        public int? NoOfPublication { get; set; }
        public bool? IsAssign { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        public bool IsAssociateFinalSubmit { get; set; }
        public System.DateTime? AnalystSubmissionDate { get; set; }
    }
}
