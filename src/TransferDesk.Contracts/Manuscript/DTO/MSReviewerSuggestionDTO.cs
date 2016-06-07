using System.Collections.Generic;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Contracts.Manuscript.DTO
{
    public class MSReviewerSuggestionDTO
    {
         public Entities.MSReviewersSuggestion MSReviewersSuggestion;
         public List<Entities.MSReviewerErrorCategory> MSReviewerErrorCategory { get; set; }
         public List<Entities.MSReviewersSuggestionInfo> msReviewerInfo { get; set; }
         public List<Entities.ReviewerErrorCategory> ErrorCategoriesList { get; set; }
         public List<ReviewerDetailsDTO> reviewerDetailsDTO { get; set; }
        
        public MSReviewerSuggestionDTO()
        {
            MSReviewersSuggestion = new TransferDesk.Contracts.Manuscript.Entities.MSReviewersSuggestion();
            msReviewerInfo = new List<Entities.MSReviewersSuggestionInfo>();
            reviewerDetailsDTO = new List<ReviewerDetailsDTO>();
            ErrorCategoriesList = new List<Entities.ReviewerErrorCategory>();
            MSReviewerErrorCategory = new List<Entities.MSReviewerErrorCategory>();
        }

        public string CurrentUserID { get; set; }
        public string UserName { get; set; }
    }
}
