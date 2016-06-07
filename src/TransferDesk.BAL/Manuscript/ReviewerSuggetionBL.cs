using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Contracts
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DTOs = TransferDesk.Contracts.Manuscript.DTO;
//Validations
using Validations = TransferDesk.BAL.Manuscript.Validations;
//UnitOfWork Manuscript Screening
using TransferDesk.DAL.Manuscript.UnitOfWork;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.Contracts.Manuscript.ComplexTypes.LocationInfo;

namespace TransferDesk.BAL.Manuscript
{
    public class ReviewerSuggetionBL : IDisposable
    {
        public ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide { get; set; }
        public ReviewerSuggestionDBRepositoryReadSide msReviewerSuggestionDBRepositoryReadSide { get; set; }
        public String _ConStringRead { get; set; }

        public String _ConStringWrite { get; set; }

        public ReviewerSuggetionBL()
        {
            //empty constructor
        }

        public ReviewerSuggetionBL(String ConStringRead, String ConStringWrite)
        {
            _ConStringRead = ConStringRead;
            _ConStringWrite = ConStringWrite;
            InitReviewerSuggetionBL();
        }
        public void InitReviewerSuggetionBL()
        {
            InitManuscriptDBRepositoryReadSide();
            InitReviewerSuggetionDBRepositoryReadSide();
        }

        public void InitManuscriptDBRepositoryReadSide()
        {
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(_ConStringWrite);
        }

        public void InitReviewerSuggetionDBRepositoryReadSide()
        {
            msReviewerSuggestionDBRepositoryReadSide = new ReviewerSuggestionDBRepositoryReadSide(_ConStringWrite);
        }

        public DTOs.MSReviewerSuggestionDTO GetMSReviewerSuggestionDefaultDTO()
        {
            DTOs.MSReviewerSuggestionDTO reviewerSuggestionDTO = new DTOs.MSReviewerSuggestionDTO();

            //reviewerSuggestionDTO.ErrorCategoriesList = _manuscriptDBRepositoryReadSide.GetErrorCategoryList();
            reviewerSuggestionDTO.ErrorCategoriesList = msReviewerSuggestionDBRepositoryReadSide.GetReviewerErrorCategoryList();

            return reviewerSuggestionDTO;

        }

        public DTOs.MSReviewerSuggestionDTO GetMSReviewerSuggestionDTO(int reviewerID)
        {

            DTOs.MSReviewerSuggestionDTO msReviewerSuggestionDTO = GetMSReviewerSuggestionDefaultDTO();
            msReviewerSuggestionDTO.MSReviewersSuggestion = msReviewerSuggestionDBRepositoryReadSide.GetManuscriptByID(reviewerID);


            msReviewerSuggestionDTO.MSReviewerErrorCategory = msReviewerSuggestionDBRepositoryReadSide.GetErrorCategoryList(reviewerID);



            return msReviewerSuggestionDTO;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //_manuscriptScreeningUnitOfWork.Dispose(); 

                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public DTOs.MSReviewerSuggestionDTO GetReviewerSuggetionDefaultDTO()
        {
            var reviewerSuggestionDTO = new DTOs.MSReviewerSuggestionDTO();

            //reviewerSuggestionDTO.ErrorCategoriesList = _manuscriptDBRepositoryReadSide.GetErrorCategoryList();
            reviewerSuggestionDTO.ErrorCategoriesList = msReviewerSuggestionDBRepositoryReadSide.GetReviewerErrorCategoryList();
            //reviewerSuggestionDTO.UserName = _manuscriptDBRepositoryReadSide.EmployeeName(userID);

            return reviewerSuggestionDTO;
        }

        public DTOs.MSReviewerSuggestionDTO GetReviewerSuggestionDTO(int? reviewerID)
        {
            DTOs.MSReviewerSuggestionDTO reviewerDTO = GetMSReviewerSuggestionDefaultDTO();
            List<DTOs.ReviewerDetailsDTO> reviewerDetailsList = new List<DTOs.ReviewerDetailsDTO>();
            reviewerDTO.MSReviewersSuggestion = msReviewerSuggestionDBRepositoryReadSide.GetManuscriptByID(reviewerID);
            reviewerDTO.msReviewerInfo = msReviewerSuggestionDBRepositoryReadSide.GetMSReviewers(reviewerDTO.MSReviewersSuggestion.ID, reviewerDTO.MSReviewersSuggestion.MSID);

            foreach (var reviewerInfo in reviewerDTO.msReviewerInfo)
            {
                DTOs.ReviewerDetailsDTO reviewerDetailsDTO = new DTOs.ReviewerDetailsDTO();

                reviewerDetailsDTO.ReviewerMaster = msReviewerSuggestionDBRepositoryReadSide.GetReviewerDetails(reviewerInfo.ReviewerMasterID);
                reviewerDetailsDTO.department = msReviewerSuggestionDBRepositoryReadSide.GetDepartmentDetails(reviewerInfo.DeptID);
                reviewerDetailsDTO.institude = msReviewerSuggestionDBRepositoryReadSide.GetInstituteDetails(reviewerInfo.InstitutionID);
                reviewerDetailsDTO.msReviewerInfoID = reviewerInfo.ID;
                List<pr_LocationInfo_Result> locationDetails = new List<pr_LocationInfo_Result>();
                if (reviewerInfo.CityID != null)
                {
                    locationDetails = msReviewerSuggestionDBRepositoryReadSide.GetLocationDetails(reviewerInfo.CityID);
                    if (locationDetails.Count > 0)
                    {
                        reviewerDetailsDTO.country = locationDetails[0].CountryName;
                        reviewerDetailsDTO.city = locationDetails[0].CityName;
                        reviewerDetailsDTO.state = locationDetails[0].StateName;
                    }
                }// added by Sambhaji Andhare.
                else
                {
                    locationDetails = msReviewerSuggestionDBRepositoryReadSide.GetLocationDetailsForCleanData(reviewerInfo.ReviewerMasterID);
                    if (locationDetails.Count > 0)
                    {
                        reviewerDetailsDTO.country = locationDetails[0].CountryName;
                        reviewerDetailsDTO.city = locationDetails[0].CityName;
                        reviewerDetailsDTO.state = locationDetails[0].StateName;
                    }
                }

                reviewerDetailsDTO.msReviewerMails = msReviewerSuggestionDBRepositoryReadSide.GetMSSReviewerMails(reviewerInfo.ID);
                reviewerDetailsDTO.msReviewerLinks = msReviewerSuggestionDBRepositoryReadSide.GetMSReviewerLinks(reviewerInfo.ID);
                reviewerDetailsDTO.msAreaOfExpertises = msReviewerSuggestionDBRepositoryReadSide.GetMSSAreaOfExpertise(reviewerInfo.ID);
                reviewerDetailsList.Add(reviewerDetailsDTO);
            }
            reviewerDTO.reviewerDetailsDTO = reviewerDetailsList;
            reviewerDTO.MSReviewerErrorCategory = msReviewerSuggestionDBRepositoryReadSide.GetErrorCategoryList(reviewerID);
            return reviewerDTO;
        }

        public bool SaveMSReviewerSuggestion(DTOs.MSReviewerSuggestionDTO msReviewerSuggestionDTO, IDictionary<string, string> dataErrors)
        {
            if (msReviewerSuggestionDTO.MSReviewersSuggestion.RoleID == 2)//todo: set constants for roles
            {
                msReviewerSuggestionDTO.MSReviewersSuggestion.QualityUserID = msReviewerSuggestionDTO.CurrentUserID;
            }
            ReviewerSuggestionUnitOfWork _reviewerSuggestionUnitOfWork = null;
            try
            {
                _reviewerSuggestionUnitOfWork = new ReviewerSuggestionUnitOfWork(_ConStringWrite);

                _reviewerSuggestionUnitOfWork.msReviewerSuggestionDTO = msReviewerSuggestionDTO;
                _reviewerSuggestionUnitOfWork.SaveMSReviewerSuggestion();
                _reviewerSuggestionUnitOfWork.SaveChanges();//todo:change this function to update ids and save as seperate commit
                return true;
            }
            //exception will be raised up in the call stack
            finally
            {
                if (_reviewerSuggestionUnitOfWork != null)
                {
                    _reviewerSuggestionUnitOfWork.Dispose();
                }
            }
        }

        public void UnAssignReviewer(int reviewerInfoID, int? msReviewersSuggestionID)
        {
            ReviewerSuggestionUnitOfWork _reviewerSuggestionUnitOfWork = null;
            _reviewerSuggestionUnitOfWork = new ReviewerSuggestionUnitOfWork(_ConStringWrite);
            _reviewerSuggestionUnitOfWork.UnAssignReviewer(reviewerInfoID, msReviewersSuggestionID);

        }

        public void GetMailDetails(Dictionary<String, String> dicReplace, int reviewerMasterID, int? msReviewersSuggestionID, string userID)
        {
            Entities.MSReviewersSuggestion msReviewersSuggestion = msReviewerSuggestionDBRepositoryReadSide.GetManuscriptByID(msReviewersSuggestionID);
            Entities.MSReviewersSuggestionInfo msReviewersSuggestionInfo = msReviewerSuggestionDBRepositoryReadSide.GetMSReviewerInfoIDs(reviewerMasterID);
            List<Entities.ReviewerMaster> reviewerName = msReviewerSuggestionDBRepositoryReadSide.GetReviewerDetails(msReviewersSuggestionInfo.ReviewerMasterID);
            var journalName = msReviewerSuggestionDBRepositoryReadSide.GetJounralName(msReviewersSuggestion.JournalID);
            Entities.Employee associateUserInfo=null;
            Entities.Employee qualityUserInfo=null;
            if(!string.IsNullOrEmpty(msReviewersSuggestion.AnalystUserID)){
                associateUserInfo = msReviewerSuggestionDBRepositoryReadSide.GetAssociateInfo(msReviewersSuggestion.AnalystUserID);
            }
            if (!string.IsNullOrEmpty(userID)){
                qualityUserInfo = msReviewerSuggestionDBRepositoryReadSide.GetAssociateInfo(userID);
            }
            dicReplace.Add("[manuscriptNumber]", msReviewersSuggestion.MSID);
            dicReplace.Add("[journalname]", journalName.ToString());
            dicReplace.Add("[reviewername]", reviewerName[0].ReviewerName);
            dicReplace.Add("[QAname]",qualityUserInfo.EmpName);
            dicReplace.Add("[Analystname]", associateUserInfo.EmpName);
            dicReplace.Add("[QAEmail]", qualityUserInfo.alternateEmail);
            dicReplace.Add("[AnalystEmail]", associateUserInfo.alternateEmail);

        
        }

        public void SaveMailDetails(Dictionary<string, string> dicReplace, string emailTo, string emailFrom, string emailCC, string emailBCC, string emailSubject, string emailBody)
        {
                //save mail details
            ReviewerSuggestionUnitOfWork _reviewerSuggestionUnitOfWork = null;
            _reviewerSuggestionUnitOfWork = new ReviewerSuggestionUnitOfWork(_ConStringWrite);
            _reviewerSuggestionUnitOfWork.SaveMailDetails(dicReplace, emailTo, emailFrom, emailCC, emailBCC, emailSubject, Convert.ToString(emailBody));
        }
    }
}
