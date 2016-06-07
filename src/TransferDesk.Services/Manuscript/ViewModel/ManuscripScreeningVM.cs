
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.DTO;
using System;


namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class ManuscripScreeningVM
    {
        private ManuscriptScreeningDTO _msDTO;
      
        internal ManuscriptScreeningDTO FetchDTO
        {
            get
            {
                ListErrorCategoryVMToDTO();
                return _msDTO;
            }
        }

        private List<ManuscriptErrorCategoryVM> _ErrorCategoryVMList;

        public ManuscripScreeningVM(ManuscriptScreeningDTO manuscriptDTO)
        {
            _msDTO = manuscriptDTO;
            ListErrorCategoryVMFromDTO();
        }

        public ManuscripScreeningVM()
        {
            _msDTO = new ManuscriptScreeningDTO();
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
            _msDTO.manuscriptErrorCategoryList = new List<ManuscriptErrorCategory>();
            //locate errorcategory in viewmodel and remove unselected with id 0 
            foreach (ManuscriptErrorCategoryVM manuscriptErrorCategoryVM in _ErrorCategoryVMList)
            {
                if (manuscriptErrorCategoryVM.ID > 0 || manuscriptErrorCategoryVM.IsSelected ==true)
                {
                    ManuscriptErrorCategory manuscriptErrorCategory = new ManuscriptErrorCategory();
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
                    _msDTO.manuscriptErrorCategoryList.Add(manuscriptErrorCategory);
                }
            }
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
            foreach(ManuscriptErrorCategory manuscriptErrorCategory in _msDTO.manuscriptErrorCategoryList)
            {
                if (manuscriptErrorCategory.IsUncheckedByUser == true) continue;//continue with next iteration
                //ManuscriptErrorCategory tempManuscriptErrorCategory = null;
                //locate errorcategory in viewmodel and fill details 
                foreach (ManuscriptErrorCategoryVM manuscriptErrorCategoryVM in _ErrorCategoryVMList)
                {
                    if(manuscriptErrorCategoryVM.ErrorCategoryID == manuscriptErrorCategory.ErrorCategoryID)
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

        [Key]
        public int ID
        { get { return _msDTO.Manuscript.ID; } set { _msDTO.Manuscript.ID = value; } }

        [Required(ErrorMessage = "Journal Title")]
        public int? JournalID
        { get { return _msDTO.Manuscript.JournalID; } set { _msDTO.Manuscript.JournalID = value; } }

        [Required(ErrorMessage = "Manuscript Number")]
        public string MSID
        { get { return _msDTO.Manuscript.MSID; } set { _msDTO.Manuscript.MSID = value; } }

        [Required(ErrorMessage = "Article Type")]
        public int? ArticleTypeID
        { get { return _msDTO.Manuscript.ArticleTypeID; } set { _msDTO.Manuscript.ArticleTypeID = value; } }

        public int? SectionID
        { get { return _msDTO.Manuscript.SectionID; } set { _msDTO.Manuscript.SectionID = value; } }

        public string UserID
        { get { return _msDTO.Manuscript.UserID; } set { _msDTO.Manuscript.UserID = value; } }
        
        [Required(ErrorMessage = "Article Title")]
        public string ArticleTitle
        { get { return _msDTO.Manuscript.ArticleTitle; } set { _msDTO.Manuscript.ArticleTitle = value; } }

        public string CorrespondingEditor
        { get { return _msDTO.Manuscript.CorrespondingEditor; } set { _msDTO.Manuscript.CorrespondingEditor = value; } }

        public string AssignedEditor
        { get { return _msDTO.Manuscript.AssignedEditor; } set { _msDTO.Manuscript.AssignedEditor = value; } }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime StartDate
        { get { return  _msDTO.Manuscript.StartDate; } set { _msDTO.Manuscript.StartDate = value; } }



        [Required(ErrorMessage = "Role")]
        public int? RoleID
        {
            get { return _msDTO.Manuscript.RoleID; }
            set { _msDTO.Manuscript.RoleID = value; }
        }
        
        [Required(ErrorMessage = "Cross check/iThenticate result %")]
        public int? Crosscheck_iThenticateResultID
        { get { return _msDTO.Manuscript.Crosscheck_iThenticateResultID; } set { _msDTO.Manuscript.Crosscheck_iThenticateResultID = value; } }

        [Required(ErrorMessage = "Highest iThenticate % from single source")]
        public decimal? Highest_iThenticateFromSingleSrc
        { get { return _msDTO.Manuscript.Highest_iThenticateFromSingleSrc; } set { _msDTO.Manuscript.Highest_iThenticateFromSingleSrc = value; } }

        [Required(ErrorMessage = "English language Quality % ")]
        public int? English_Lang_QualityID
        { get { return _msDTO.Manuscript.English_Lang_QualityID; } set { _msDTO.Manuscript.English_Lang_QualityID = value; } }

        [Required(ErrorMessage = "Ethics Complience %")]
        public int? Ethics_ComplianceID
        { get { return _msDTO.Manuscript.Ethics_ComplianceID; } set { _msDTO.Manuscript.Ethics_ComplianceID = value; } }
       
        public string Conclusion
        { get { return _msDTO.Manuscript.Conclusion; } set { _msDTO.Manuscript.Conclusion = value; } }

       
        public string TransferFrom
        { get { return _msDTO.Manuscript.TransferFrom; } set { _msDTO.Manuscript.TransferFrom = value; } }
       
        public string ReviewerComments
        { get { return _msDTO.Manuscript.ReviewerComments; } set { _msDTO.Manuscript.ReviewerComments = value; } }
       
        public string Abstarct
        { get { return _msDTO.Manuscript.Abstarct; } set { _msDTO.Manuscript.Abstarct = value; } }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public System.DateTime InitialSubmissionDate
        { get { return _msDTO.Manuscript.InitialSubmissionDate; } set { _msDTO.Manuscript.InitialSubmissionDate = value; } }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public System.DateTime? QualityStartCheckDate
        { get { return _msDTO.Manuscript.QualityStartCheckDate; } set { _msDTO.Manuscript.QualityStartCheckDate = value; } }
       
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public System.DateTime? QualityEndDate
        { get { return _msDTO.Manuscript.QualityEndDate; } set { _msDTO.Manuscript.QualityEndDate = value; } }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public System.DateTime? FinalSubmitDate
        { get { return _msDTO.Manuscript.FinalSubmitDate; } set { _msDTO.Manuscript.FinalSubmitDate = value; } }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public System.DateTime? QualityTAT
        { get { return _msDTO.Manuscript.QualityTAT; } set { _msDTO.Manuscript.QualityTAT = value; } }
       
        public int Status { get { return _msDTO.Manuscript.Status; } set { _msDTO.Manuscript.Status = value; } }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public System.DateTime? ModifiedDateTime
        { get { return _msDTO.Manuscript.ModifiedDateTime; } set { _msDTO.Manuscript.ModifiedDateTime = value; } }

        [Required(ErrorMessage = "Corresponding Author")]
        public string CorrespondingAuthor
        { get { return _msDTO.Manuscript.CorrespondingAuthor; } set { _msDTO.Manuscript.CorrespondingAuthor = value; } }

        //[DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email")]
        [RegularExpression(@"(([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*;\s*|\s*$))*", ErrorMessage = "Email is not valid")]
        public string CorrespondingAuthorEmail
        { get { return _msDTO.Manuscript.CorrespondingAuthorEmail; } set { _msDTO.Manuscript.CorrespondingAuthorEmail = value; } }

        [Required(ErrorMessage = "Corresponding Author Affiliation")]
        public string CorrespondingAuthorAff
        { get { return _msDTO.Manuscript.CorrespondingAuthorAff; } set { _msDTO.Manuscript.CorrespondingAuthorAff = value; } }

        [Required(ErrorMessage = "iThenticate %")]
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

      
        public int? OverallAnalysisID
        { get { return _msDTO.Manuscript.OverallAnalysisID; } set { _msDTO.Manuscript.OverallAnalysisID = value; } }

        public bool? IsAccurate
        { get { return _msDTO.Manuscript.IsAccurate; } set { _msDTO.Manuscript.IsAccurate = value; } }

        public string Comments_English_Lang_Quality
        { get { return _msDTO.Manuscript.Comments_English_Lang_Quality; } set { _msDTO.Manuscript.Comments_English_Lang_Quality = value; } }

        public string Comments_Ethics_Compliance
        { get { return _msDTO.Manuscript.Comments_Ethics_Compliance; } set { _msDTO.Manuscript.Comments_Ethics_Compliance = value; } }

        public string Comments_Crosscheck_iThenticateResult
        { get { return _msDTO.Manuscript.Comments_Crosscheck_iThenticateResult; } set { _msDTO.Manuscript.Comments_Crosscheck_iThenticateResult = value; } }

        public bool AddedNewRevision
        { get { return _msDTO.AddedNewRevision; } set { _msDTO.AddedNewRevision = value; } }

        public System.DateTime? RevisedDate
        { get { return _msDTO.Manuscript.RevisedDate; } set { _msDTO.Manuscript.RevisedDate = value; } }

        public string Comments_OverallAnalysis
        { get { return _msDTO.Manuscript.Comments_OverallAnalysis; } set { _msDTO.Manuscript.Comments_OverallAnalysis = value; } }

        public bool? IsAssociateFinalSubmit
        { get { return _msDTO.Manuscript.IsAssociateFinalSubmit; } set { _msDTO.Manuscript.IsAssociateFinalSubmit = value; } }

        public bool? IsQualityFinalSubmit
        { get { return _msDTO.Manuscript.IsQualityFinalSubmit; } set { _msDTO.Manuscript.IsQualityFinalSubmit = value; } }

        public string HandlingEditor
        { get { return _msDTO.Manuscript.HandlingEditor; } set { _msDTO.Manuscript.HandlingEditor = value; } }
        public int? ParentManuscriptID
        { get { return _msDTO.Manuscript.ParentManuscriptID; } set { _msDTO.Manuscript.ParentManuscriptID = value; } }
        public int? JournalStatusID
        {
            get { return _msDTO.Manuscript.JournalStatusID; }
            set { _msDTO.Manuscript.JournalStatusID = value; }
        }


    }
}

