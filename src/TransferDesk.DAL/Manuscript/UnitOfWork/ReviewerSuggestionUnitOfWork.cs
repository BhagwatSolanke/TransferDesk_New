using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Data Access Layer
using Repos = TransferDesk.DAL.Manuscript.Repositories;
using DContext = TransferDesk.DAL.Manuscript.DataContext;

//Contracts
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DTOs = TransferDesk.Contracts.Manuscript.DTO;

//important to note unitofwork saves work and will have a seperate context, so that service can have lifetime instance

namespace TransferDesk.DAL.Manuscript.UnitOfWork
{
   public class ReviewerSuggestionUnitOfWork:IDisposable
    {
        private Repos.MSReviewersSuggestion _msReviewersSuggestion;
        private Repos.MSReviewerInfo _msReviewerInfo;
        private Repos.MSReviewerErrorCategory _manuscriptErrorCategoryRepository;
        private Repos.ErrorCategoryRepository _errorCategoryRepository;
        private Repos.EmailDetailsRepository _emailDetailsRepository;
        public DTOs.MSReviewerSuggestionDTO msReviewerSuggestionDTO{ get; set; }
        Entities.EmailDetails _emailDetails;

        public ReviewerSuggestionUnitOfWork(string conString)
        {
            _msReviewersSuggestion = new Repos.MSReviewersSuggestion(conString);
            _errorCategoryRepository = new Repos.ErrorCategoryRepository(_msReviewersSuggestion.context);
            _manuscriptErrorCategoryRepository = new Repos.MSReviewerErrorCategory(_msReviewersSuggestion.context);
            _msReviewerInfo = new Repos.MSReviewerInfo(conString);
            _emailDetailsRepository = new Repos.EmailDetailsRepository(conString);
        }

          private void SaveManuscriptErrorCategories()
        {
            if (msReviewerSuggestionDTO.MSReviewerErrorCategory!= null)
            {
                foreach (Entities.MSReviewerErrorCategory manuscriptErrorCategory in msReviewerSuggestionDTO.MSReviewerErrorCategory)
                {
                    //viewmodel is defined and sent so sure that it is 0(or value) and it is not null
                    if (manuscriptErrorCategory.ID == 0)
                    {
                        manuscriptErrorCategory.MSReviewersSuggestionID =msReviewerSuggestionDTO.MSReviewersSuggestion.ID;
                        manuscriptErrorCategory.MSID = msReviewerSuggestionDTO.MSReviewersSuggestion.MSID;
                        _manuscriptErrorCategoryRepository.AddMSReviewerErrorCategory(manuscriptErrorCategory);
                    }
                    else
                    {
                        manuscriptErrorCategory.MSReviewersSuggestionID =msReviewerSuggestionDTO.MSReviewersSuggestion.ID;
                        manuscriptErrorCategory.MSID = msReviewerSuggestionDTO.MSReviewersSuggestion.MSID;
                        _manuscriptErrorCategoryRepository.UpdateMSReviewerErrorCategory(manuscriptErrorCategory);
                    }
                }
            }
        }


          public void SaveMSReviewerSuggestion()
          {
              SaveManuscript();
              SaveManuscriptErrorCategories();
          }

          private void SaveManuscript()
          {
              if (msReviewerSuggestionDTO.MSReviewersSuggestion.ID == 0) // first sent starting from create new then
              {
                  //_manuscriptRepository.AddManuscript(manuscriptScreeningDTO.Manuscript);
              }
              else
              {
                  //_manuscriptRepository.UpdateManuscript(manuscriptScreeningDTO.Manuscript);
                  _msReviewersSuggestion.UpdateMSReviewersSuggestion(msReviewerSuggestionDTO.MSReviewersSuggestion);
              }
          }

        

          public void SaveChanges()
          {
              //loop to update ids from first manuscript Save and finally save context
              _msReviewersSuggestion.SaveChanges();
          }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //todo:check null to check if instance is created
                    //_otherAuthorsRepository.Dispose();
                    //_errorCategoryRepository.Dispose();
                    //_manuscriptErrorCategoryRepository.Dispose();
                    //_manuscriptRepository.Dispose();

                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public void UnAssignReviewer(int reviewerInfoID, int? msReviewersSuggestionID)
        {
            _msReviewerInfo.UnAssignReviewer(reviewerInfoID, msReviewersSuggestionID);
        }

        public void SaveMailDetails(Dictionary<string, string> dicReplace, string emailTo, string emailFrom, string emailCC, string emailBCC, string emailSubject, string emailBody)
        {
             _emailDetails= new Entities.EmailDetails();
            _emailDetails.MSID = Convert.ToString(dicReplace["[manuscriptNumber]"]);
            _emailDetails.To = emailTo;
            _emailDetails.From = emailFrom;
            _emailDetails.Subject = emailSubject;
            _emailDetails.Body = emailBody;
            _emailDetails.CC = emailCC;
            _emailDetails.BCC = emailBCC;
            _emailDetails.SendDateTime = DateTime.Now;
            _emailDetails.Status = true;
            _emailDetailsRepository.AddEmailDetail(_emailDetails);
        }
    }
}
