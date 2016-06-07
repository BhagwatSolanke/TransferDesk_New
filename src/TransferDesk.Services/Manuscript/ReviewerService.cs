using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entities = TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.BAL.Manuscript;
using TransferDesk.Services.Manuscript.ViewModel;
using System.Collections;

//todo: a seperate "web adapter" for service that will allow modelstate to cross will be created.

namespace TransferDesk.Services.Manuscript
{
    public class ReviewerService
    {
        public String _ConStringRead { get; set; }

        public String _ConStringWrite { get; set; }

        public ReviewerSuggetionBL _reviewerSuggetionBL { get; set; }
           
        public ReviewerService()
        {
            //empty constructor            
        }

        public ReviewerService(String ConStringRead, String ConStringWrite)
        {
            _ConStringRead = ConStringRead;
            _ConStringWrite = ConStringWrite;
            CreateReviewerSuggetionBL();
        }

        public void CreateReviewerSuggetionBL()
        {
            _reviewerSuggetionBL = new ReviewerSuggetionBL(_ConStringRead, _ConStringWrite);
        }

        public ReviewerSuggestionVM GetManuscriptScreeningDefaultVM()
        {
            MSReviewerSuggestionDTO reviewerSuggetionDTO = _reviewerSuggetionBL.GetReviewerSuggetionDefaultDTO();
            //reviewerSuggetionDTO.MSReviewersSuggestion.AnalystUserID= System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            return new ReviewerSuggestionVM(reviewerSuggetionDTO);
        }

        public ReviewerSuggestionVM GetManuscriptScreeningVM(int? reviewerID)
        {
            return new ReviewerSuggestionVM(_reviewerSuggetionBL.GetReviewerSuggestionDTO(reviewerID));
        }

        public bool SaveMSReviewerSuggestionVM(IDictionary<string, string> dataErrors, ReviewerSuggestionVM msReviewerSuggestionVM)
        {
            MSReviewerSuggestionDTO msReviewerSuggestionDTO = msReviewerSuggestionVM.FetchDTO;
            msReviewerSuggestionDTO.CurrentUserID = System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            ValidateMSReviewerSuggestion(dataErrors, msReviewerSuggestionDTO);
            if (dataErrors.Count == 0)
            {
                _reviewerSuggetionBL.SaveMSReviewerSuggestion(msReviewerSuggestionDTO, dataErrors);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ValidateMSReviewerSuggestion(IDictionary<string, string> dataErrors,MSReviewerSuggestionDTO msReviewerSuggestionDTO)
        {
            Entities.MSReviewersSuggestion msReviewersSuggestion = msReviewerSuggestionDTO.MSReviewersSuggestion;
            if(msReviewersSuggestion.StartDate==null)
                dataErrors.Add("StartDate", "Start Date is required.");
            if(msReviewersSuggestion.JournalID==null)
                dataErrors.Add("JournalID", "JournalTitle is required.");
            if (msReviewersSuggestion.MSID == null)
                dataErrors.Add("MSID", "Manuscript Number is required.");
            if (msReviewersSuggestion.ArticleTitle== null)
                dataErrors.Add("ArticleTitle", "Article Title is required.");
            if (msReviewersSuggestion.RoleID == null)
                dataErrors.Add("RoleID", "Role is required.");
        }

        public bool UnAssignReviewer(int reviewerInfoID, int? msReviewersSuggestionID)
        {
            _reviewerSuggetionBL.UnAssignReviewer(reviewerInfoID, msReviewersSuggestionID);
            return true;
        }

        public void GetMailDetails(Dictionary<String, String> dicReplace,int reviewerInfoID, int? msReviewersSuggestionID, string userID)
        {
            _reviewerSuggetionBL.GetMailDetails(dicReplace,reviewerInfoID, msReviewersSuggestionID, userID);
        }

        public void SaveMailDetails(Dictionary<string, string> dicReplace, string emailTo, string emailFrom, string emailCC, string emailBCC, string emailSubject, string emailBody)
        {
            _reviewerSuggetionBL.SaveMailDetails(dicReplace,emailTo, emailFrom, emailCC, emailBCC, emailSubject, Convert.ToString(emailBody));
        }
    }
}
