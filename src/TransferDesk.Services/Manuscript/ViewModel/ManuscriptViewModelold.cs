
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.DTO;
using System;


namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class ManuscriptViewModel
    {
        private ManuscriptDTO _msDTO;
        internal ManuscriptDTO msDTO 
        {
            get
            {
                ListErrorCategoryVMToDTO();
                return _msDTO;
            }

            //set
            //{
            //    _msDTO = value;
            //}
            
        }
        private List<ManuscriptErrorCategoryVM> _ErrorCategoryVMList;

        public ManuscriptViewModel(ManuscriptDTO manuscriptDTO)
        {
            _msDTO = manuscriptDTO;
            ListErrorCategoryVMFromDTO();
        }

        public ManuscriptViewModel()
        {
            _msDTO = new ManuscriptDTO();
           
        }

        public List<OtherAuthor> OtherAuthors
        { get { return _msDTO.OtherAuthors; } set { _msDTO.OtherAuthors = value; } }

        public List<ManuscriptErrorCategoryVM> ListManuscriptErrorCategoriesVM
        {
            get 
            {
                return _ErrorCategoryVMList;
            }
            //todo:during set fill dto using value that comes in as list
            set
            { 
                _ErrorCategoryVMList = value;
                //ListErrorCategoryVMToDTO();
            }
        }

        private void ListErrorCategoryVMToDTO()
        {
            
            //locate errorcategory in viewmodel and remove unselected with id 0 
            foreach (ManuscriptErrorCategoryVM manuscriptErrorCategoryVM in _ErrorCategoryVMList)
            {
                if (manuscriptErrorCategoryVM.ID > 0 || manuscriptErrorCategoryVM.IsSelected ==true)
                {
                    ManuscriptErrorCategory manuscriptErrorCategory = new ManuscriptErrorCategory();
                    manuscriptErrorCategory.ID = manuscriptErrorCategoryVM.ID;
                    manuscriptErrorCategory.ErrorCategoryID = manuscriptErrorCategoryVM.ErrorCategoryID;

                    _msDTO.manuscriptErrorCategoryList.Add(manuscriptErrorCategory);
                }
            }

            //_ErrorCategoryVMList = ErrorCategoryVMListNew;
        }


        private List<ManuscriptErrorCategoryVM> ListErrorCategoryVMFromDTO()
        {
             _ErrorCategoryVMList = new List<ManuscriptErrorCategoryVM>();
             
            //First fetch error categories master from DTO into each VM
            foreach(ErrorCategory errorCategory in _msDTO.ErrorCategoriesList)
            {
                ManuscriptErrorCategoryVM manuscriptErrorCategoryVM = new ManuscriptErrorCategoryVM();
                   manuscriptErrorCategoryVM.ErrorCategoryID = errorCategory.ID;
                   manuscriptErrorCategoryVM.ErrorCategoryName = errorCategory.ErrorCategoryName;

                   _ErrorCategoryVMList.Add(manuscriptErrorCategoryVM);
            }

            //todo:Now update the already selected in dto into list  if any
            foreach(ManuscriptErrorCategory manuscriptErrorCategory in _msDTO.manuscriptErrorCategoryList   )
            {
                //locate errorcategory in viewmodel and fill details 
                foreach (ManuscriptErrorCategoryVM manuscriptErrorCategoryVM in _ErrorCategoryVMList)
                {
                    if(manuscriptErrorCategoryVM.ErrorCategoryID == manuscriptErrorCategory.ErrorCategoryID)
                    {
                        //fill all details from manuscript error categories
                        manuscriptErrorCategoryVM.IsSelected = true;
                        manuscriptErrorCategoryVM.ID = manuscriptErrorCategoryVM.ID;
                    }
                }

            }

            return _ErrorCategoryVMList;

        }

        [Key]
        public int? ID
        { get { return _msDTO.Manuscript.ID; } set { _msDTO.Manuscript.ID = value; } }

        public int? JournalID
        { get { return _msDTO.Manuscript.JournalID; } set { _msDTO.Manuscript.JournalID = value; } }

        public int? MSID
        { get { return _msDTO.Manuscript.MSID; } set { _msDTO.Manuscript.MSID = value; } }

        public int? ArticleTypeID
        { get { return _msDTO.Manuscript.ArticleTypeID; } set { _msDTO.Manuscript.ArticleTypeID = value; } }
        public int? SectionID

        { get { return _msDTO.Manuscript.SectionID; } set { _msDTO.Manuscript.SectionID = value; } }
        [Required(ErrorMessage = "Please, enter Article title")]
        
        public string ArticleTitle
        { get { return _msDTO.Manuscript.ArticleTitle; } set { _msDTO.Manuscript.ArticleTitle = value; } }

        [Required(ErrorMessage = "CorrespondingEditor is required")]
        public string CorrespondingEditor
        { get { return _msDTO.Manuscript.CorrespondingEditor; } set { _msDTO.Manuscript.CorrespondingEditor = value; } }

        [Required(ErrorMessage = "AssignedEditor is required")]
        public string AssignedEditor
        { get { return _msDTO.Manuscript.AssignedEditor; } set { _msDTO.Manuscript.AssignedEditor = value; } }
        
        public System.DateTime StartDate
        { get { return _msDTO.Manuscript.StartDate; } set { _msDTO.Manuscript.StartDate = value; } }

        [Required(ErrorMessage = "Please, select Role")]
        public int? RoleID
        {
            get { return _msDTO.Manuscript.RoleID; }
            set { _msDTO.Manuscript.RoleID = value; }
        }
        
        public string UserID
        { get { return _msDTO.Manuscript.UserID; } set { _msDTO.Manuscript.UserID = value; } }
        
        public int? Crosscheck_iThenticateResultID
        { get { return _msDTO.Manuscript.Crosscheck_iThenticateResultID; } set { _msDTO.Manuscript.Crosscheck_iThenticateResultID = value; } }
        
        public decimal? Highest_iThenticateFromSingleSrc
        { get { return _msDTO.Manuscript.Highest_iThenticateFromSingleSrc; } set { _msDTO.Manuscript.Highest_iThenticateFromSingleSrc = value; } }
       
        public int English_Lang_QualityID
        { get { return _msDTO.Manuscript.English_Lang_QualityID; } set { _msDTO.Manuscript.English_Lang_QualityID = value; } }
       
        public string Conclusion
        { get { return _msDTO.Manuscript.Conclusion; } set { _msDTO.Manuscript.Conclusion = value; } }
        
        public int Ethics_ComplianceID
        { get { return _msDTO.Manuscript.Ethics_ComplianceID; } set { _msDTO.Manuscript.Ethics_ComplianceID = value; } }
       
        public string TransferFrom
        { get { return _msDTO.Manuscript.TransferFrom; } set { _msDTO.Manuscript.TransferFrom = value; } }
       
        public string ReviewerComments
        { get { return _msDTO.Manuscript.ReviewerComments; } set { _msDTO.Manuscript.ReviewerComments = value; } }
       
        public string Abstarct
        { get { return _msDTO.Manuscript.Abstarct; } set { _msDTO.Manuscript.Abstarct = value; } }
       
        public System.DateTime InitialSubmissionDate
        { get { return _msDTO.Manuscript.InitialSubmissionDate; } set { _msDTO.Manuscript.InitialSubmissionDate = value; } }
        
        public System.DateTime? AssociateTAT
        { get { return _msDTO.Manuscript.AssociateTAT; } set { _msDTO.Manuscript.AssociateTAT = value; } }

        public bool? QualityCheck
        {
            get
            {
                return _msDTO.Manuscript.QualityCheck.HasValue ? _msDTO.Manuscript.QualityCheck.Value : false;
            }
            set{       
                _msDTO.Manuscript.QualityCheck = value;}
        }

        public System.DateTime? QualityStartCheckDate
        { get { return _msDTO.Manuscript.QualityStartCheckDate; } set { _msDTO.Manuscript.QualityStartCheckDate = value; } }
        //public int? ErrorCategoryID 
        //{ get { return _msDTO.Manuscript.ErrorCategoryID; } set { _msDTO.Manuscript.ErrorCategoryID = value; } }
       
        public System.DateTime? QualityEndDate
        { get { return _msDTO.Manuscript.QualityEndDate; } set { _msDTO.Manuscript.QualityEndDate = value; } }
        
        public System.DateTime? FinalSubmitDate
        { get { return _msDTO.Manuscript.FinalSubmitDate; } set { _msDTO.Manuscript.FinalSubmitDate = value; } }
        
        public System.DateTime? QualityTAT
        { get { return _msDTO.Manuscript.QualityTAT; } set { _msDTO.Manuscript.QualityTAT = value; } }
       
        public int Status { get { return _msDTO.Manuscript.Status; } set { _msDTO.Manuscript.Status = value; } }
        
        public System.DateTime? ModifiedDateTime
        { get { return _msDTO.Manuscript.ModifiedDateTime; } set { _msDTO.Manuscript.ModifiedDateTime = value; } }

        [Required(ErrorMessage = "CorrespondingAuthor is required")]
        public string CorrespondingAuthor
        { get { return _msDTO.Manuscript.CorrespondingAuthor; } set { _msDTO.Manuscript.CorrespondingAuthor = value; } }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string CorrespondingAuthorEmail
        { get { return _msDTO.Manuscript.CorrespondingAuthorEmail; } set { _msDTO.Manuscript.CorrespondingAuthorEmail = value; } }

        [Required(ErrorMessage = "CorrespondingAuthorAff is required")]
        public string CorrespondingAuthorAff
        { get { return _msDTO.Manuscript.CorrespondingAuthorAff; } set { _msDTO.Manuscript.CorrespondingAuthorAff = value; } }
        
        public decimal? iThenticatePercentage
        { get { return _msDTO.Manuscript.iThenticatePercentage; } set { _msDTO.Manuscript.iThenticatePercentage = value; } }
       
        public string OverallAnalysis
        { get { return _msDTO.Manuscript.OverallAnalysis; } set { _msDTO.Manuscript.OverallAnalysis = value; } }
       
        public bool? HasTransferReport
        { get { return _msDTO.Manuscript.HasTransferReport; } set { _msDTO.Manuscript.HasTransferReport = value; } }
        
        public string Accurate
        { get { return _msDTO.Manuscript.Accurate; } set { _msDTO.Manuscript.Accurate = value; } }
       
        public string ErrorDescription
        { get { return _msDTO.Manuscript.ErrorDescription; } set { _msDTO.Manuscript.ErrorDescription = value; } }


    }
}

