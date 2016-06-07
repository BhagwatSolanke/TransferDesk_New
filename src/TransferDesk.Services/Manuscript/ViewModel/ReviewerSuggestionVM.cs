using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.DTO;
using System;
using System.Linq;


namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class ReviewerSuggestionVM
    {
        private MSReviewerSuggestionDTO _msDTO;
        internal MSReviewerSuggestionDTO FetchDTO
        {
            get
            {
                ListErrorCategoryVMToDTO();
                return _msDTO;
            }
        }

        private List<ReviewerErrorCategoryVM> _ErrorCategoryVMList;

        public ReviewerSuggestionVM(MSReviewerSuggestionDTO msReviewerSuggestionDTO)
        {
            _msDTO = msReviewerSuggestionDTO;
            ManuscriptReviewers();
            ListErrorCategoryVMFromDTO();
        }

        public List<MSIDReviewersVM> MSIDReviewersVM;

        private List<MSIDReviewersVM> ManuscriptReviewers()
        {
            MSIDReviewersVM = new List<MSIDReviewersVM>();
            int count = 0;

            foreach (var reviewerInfo in _msDTO.reviewerDetailsDTO)
            {
                var reviewerVM = new MSIDReviewersVM();
                reviewerVM.AreaOfExpertise = reviewerInfo.msAreaOfExpertises;
                reviewerVM.ReferenceLinks = reviewerInfo.msReviewerLinks;
                reviewerVM.EmailAddress = reviewerInfo.msReviewerMails;
                reviewerVM.City = reviewerInfo.city;
                reviewerVM.Country = reviewerInfo.country;
                reviewerVM.Department = reviewerInfo.department;
                reviewerVM.Institution = reviewerInfo.institude;
                ///reviewerVM.NoOfPublication = reviewerInfo.ReviewerMaster[count].NoOfPublication;
                var noOfPublication = (from msReviewerInfo in _msDTO.msReviewerInfo
                                       join reviewerMaster in reviewerInfo.ReviewerMaster on msReviewerInfo.ReviewerMasterID equals reviewerMaster.ID
                                       where msReviewerInfo.ID == reviewerInfo.msReviewerInfoID
                                       select msReviewerInfo.NoOfPublication).FirstOrDefault();
                reviewerVM.NoOfPublication = noOfPublication;

                reviewerVM.Initials = reviewerInfo.ReviewerMaster[count].Initials != null ? reviewerInfo.ReviewerMaster[count].Initials : string.Empty;
                reviewerVM.FirstName = reviewerInfo.ReviewerMaster[count].FirstName;
                reviewerVM.LastName = reviewerInfo.ReviewerMaster[count].LastName;
                reviewerVM.StreetName = reviewerInfo.ReviewerMaster[count].StreetName;
                reviewerVM.MiddleName = reviewerInfo.ReviewerMaster[count].MiddleName;
                reviewerVM.ID = reviewerInfo.msReviewerInfoID;
                MSIDReviewersVM.Add(reviewerVM);
            }
            return MSIDReviewersVM;
        }

        private List<ReviewerErrorCategoryVM> ListErrorCategoryVMFromDTO()
        {
            _ErrorCategoryVMList = new List<ReviewerErrorCategoryVM>();

            //First fetch error categories master from DTO into each VM
            foreach (ReviewerErrorCategory errorCategory in _msDTO.ErrorCategoriesList)
            {
                var manuscriptErrorCategoryVM = new ReviewerErrorCategoryVM();
                manuscriptErrorCategoryVM.ErrorCategoryID = errorCategory.ID;
                manuscriptErrorCategoryVM.ErrorCategoryName = errorCategory.ErrorCategoryName;
                _ErrorCategoryVMList.Add(manuscriptErrorCategoryVM);
            }

            //todo:Now update the already selected in dto into list  if any
            foreach (MSReviewerErrorCategory manuscriptErrorCategory in _msDTO.MSReviewerErrorCategory)
            {
                if (manuscriptErrorCategory.IsUncheckedByUser == true) continue;//continue with next iteration
                //ManuscriptErrorCategory tempManuscriptErrorCategory = null;
                //locate errorcategory in viewmodel and fill details 
                foreach (ReviewerErrorCategoryVM manuscriptErrorCategoryVM in _ErrorCategoryVMList)
                {
                    if (manuscriptErrorCategoryVM.ErrorCategoryID == manuscriptErrorCategory.ErrorCategoryID)
                    {
                        //fill all details from manuscript error categories
                        manuscriptErrorCategoryVM.IsSelected = true;
                        manuscriptErrorCategoryVM.ID = manuscriptErrorCategory.ID;
                        //tempManuscriptErrorCategory = manuscriptErrorCategory;
                    }
                }
            }
            return _ErrorCategoryVMList;
        }


        private void ListErrorCategoryVMToDTO()
        {
            _msDTO.MSReviewerErrorCategory = new List<MSReviewerErrorCategory>();
            //locate errorcategory in viewmodel and remove unselected with id 0 
            foreach (ReviewerErrorCategoryVM manuscriptErrorCategoryVM in _ErrorCategoryVMList)
            {
                if (manuscriptErrorCategoryVM.ID > 0 || manuscriptErrorCategoryVM.IsSelected == true)
                {
                    MSReviewerErrorCategory manuscriptErrorCategory = new MSReviewerErrorCategory();
                    manuscriptErrorCategory.ID = manuscriptErrorCategoryVM.ID;
                    manuscriptErrorCategory.ErrorCategoryID = manuscriptErrorCategoryVM.ErrorCategoryID;
                    if (manuscriptErrorCategoryVM.ID > 0 && manuscriptErrorCategoryVM.IsSelected == false)
                    {
                        //todo: remove unchecked by user on progressive updates, instead of deletion
                        manuscriptErrorCategory.IsUncheckedByUser = true;
                    }
                    if (manuscriptErrorCategoryVM.ID > 0 && manuscriptErrorCategoryVM.IsSelected == true)
                    {
                        //todo: remove unchecked by user on progressive updates, instead of deletion
                        manuscriptErrorCategory.IsUncheckedByUser = false;
                    }
                    _msDTO.MSReviewerErrorCategory.Add(manuscriptErrorCategory);
                }
            }
        }

        public ReviewerSuggestionVM()
        {
            _msDTO = new MSReviewerSuggestionDTO();
        }

        public List<ReviewerErrorCategoryVM> ListManuscriptErrorCategoriesVM
        {
            get
            {
                return _ErrorCategoryVMList;
            }
            set
            {
                _ErrorCategoryVMList = value;
            }
        }

        public bool? QualityCheck
        {
            get
            {
                return _msDTO.MSReviewersSuggestion.QualityCheck.HasValue ? _msDTO.MSReviewersSuggestion.QualityCheck.Value : false;
            }
            set
            {
                _msDTO.MSReviewersSuggestion.QualityCheck = value;
            }
        }


        [Key]
        public int ID
        { get { return _msDTO.MSReviewersSuggestion.ID; } set { _msDTO.MSReviewersSuggestion.ID = value; } }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime? StartDate
        { get { return _msDTO.MSReviewersSuggestion.StartDate; } set { _msDTO.MSReviewersSuggestion.StartDate = value; } }

        [Required(ErrorMessage = "Journal Title")]
        public int? JournalID
        { get { return _msDTO.MSReviewersSuggestion.JournalID; } set { _msDTO.MSReviewersSuggestion.JournalID = value; } }

        [Required(ErrorMessage = "Manuscript Number")]
        public string MSID
        { get { return _msDTO.MSReviewersSuggestion.MSID; } set { _msDTO.MSReviewersSuggestion.MSID = value; } }

        [Required(ErrorMessage = "Article Title")]
        public string ArticleTitle
        { get { return _msDTO.MSReviewersSuggestion.ArticleTitle; } set { _msDTO.MSReviewersSuggestion.ArticleTitle = value; } }

        [Required(ErrorMessage = "Role")]
        public int RoleID
        {
            get { return _msDTO.MSReviewersSuggestion.RoleID; }
            set { _msDTO.MSReviewersSuggestion.RoleID = value; }
        }

        [Required(ErrorMessage = "TaskID")]
        public int SMTaskID
        {
            get { return _msDTO.MSReviewersSuggestion.SMTaskID; }
            set { _msDTO.MSReviewersSuggestion.SMTaskID = value; }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public System.DateTime? QualityStartCheckDate
        { get { return _msDTO.MSReviewersSuggestion.QualityStartDate; } set { _msDTO.MSReviewersSuggestion.QualityStartDate = value; } }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public System.DateTime? QualityEndDate
        { get { return _msDTO.MSReviewersSuggestion.QualitySubmissionDate; } set { _msDTO.MSReviewersSuggestion.QualitySubmissionDate = value; } }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public System.DateTime? QualityTAT
        { get { return _msDTO.MSReviewersSuggestion.QualityTAT; } set { _msDTO.MSReviewersSuggestion.QualityTAT = value; } }

        public bool? Accurate
        { get { return _msDTO.MSReviewersSuggestion.IsAccurate; } set { _msDTO.MSReviewersSuggestion.IsAccurate = value; } }

        public string ErrorDescription
        { get { return _msDTO.MSReviewersSuggestion.ErrorDescription; } set { _msDTO.MSReviewersSuggestion.ErrorDescription = value; } }

        public bool? IsAssociateFinalSubmit
        { get { return _msDTO.MSReviewersSuggestion.IsAssociateFinalSubmit; } set { _msDTO.MSReviewersSuggestion.IsAssociateFinalSubmit = value; } }

        public bool? IsQualityFinalSubmit
        { get { return _msDTO.MSReviewersSuggestion.IsQualityFinalSubmit; } set { _msDTO.MSReviewersSuggestion.IsQualityFinalSubmit = value; } }

        public string UserName { get { return _msDTO.UserName; } set { _msDTO.UserName = value; } }

        public string AnalystUserID { get { return _msDTO.MSReviewersSuggestion.AnalystUserID; } set { _msDTO.MSReviewersSuggestion.AnalystUserID = value; } }
    }
}
