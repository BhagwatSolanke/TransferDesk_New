using System.Collections.Generic;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Contracts.Manuscript.DTO
{
    public class ReviewerDetailsDTO
    {
        public List<Entities.ReviewerMaster> ReviewerMaster { get; set; }

        public string department { get; set; }
        public string institude { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public int msReviewerInfoID { get; set; }
        public List<Entities.MSReviewerLink> msReviewerLinks { get; set; }
        public List<Entities.MSSAreaOfExpertise> msAreaOfExpertises { get; set; }
        public List<Entities.MSSReviewerMail> msReviewerMails { get; set; }

        public ReviewerDetailsDTO()
        {
            ReviewerMaster = new List<Entities.ReviewerMaster>();
            msReviewerLinks = new List<Entities.MSReviewerLink>();
            msAreaOfExpertises = new List<Entities.MSSAreaOfExpertise>();
            msReviewerMails = new List<Entities.MSSReviewerMail>();
        }
    }
}
