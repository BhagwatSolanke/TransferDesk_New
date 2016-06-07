using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Entities;
namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class MSIDReviewersVM
    {
        public int ID { get; set; }
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Institution { get; set; }
        public string Department { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<MSSReviewerMail> EmailAddress { get; set; }
        public List<MSSAreaOfExpertise> AreaOfExpertise { get; set; }
        public List<MSReviewerLink> ReferenceLinks { get; set; }
        public int? NoOfPublication { get; set; }
    }
}
