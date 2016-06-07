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
    public class ManuscriptService
    {
        public String _ConStringRead { get; set; }

        public String _ConStringWrite { get; set; }

        public ManuscriptScreeningBL _manuscriptScreeningBL { get; set; }
           
        public ManuscriptService()
        {
            //empty constructor            
        }

        public ManuscriptService(String ConStringRead, String ConStringWrite)
        {
            _ConStringRead = ConStringRead;
            _ConStringWrite = ConStringWrite;
            CreateManuscriptScreeningBL();
        }

        public void CreateManuscriptScreeningBL()
        {
            _manuscriptScreeningBL = new ManuscriptScreeningBL(_ConStringRead, _ConStringWrite);
        }
        
        public void CreateManuscriptServiceComponents()
        {
            CreateManuscriptScreeningBL();
        }

        public ManuscripScreeningVM GetManuscriptScreeningVM()
        {
           return GetManuscriptScreeningDefaultVM();
        }

        public ManuscripScreeningVM GetManuscriptScreeningVM(int manuscriptID)
        {
           return new ManuscripScreeningVM(_manuscriptScreeningBL.GetManuscriptScreeningDTO(manuscriptID));
        }

        public ManuscripScreeningVM GetManuscriptScreeningDefaultVM()
        {
            ManuscriptScreeningDTO manuscriptScreeningDTO = _manuscriptScreeningBL.GetManuscriptScreeningDefaultDTO();
            manuscriptScreeningDTO.Manuscript.UserID = System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            return new ManuscripScreeningVM(manuscriptScreeningDTO);
        }

        public ManuscriptBookScreeningVm GetManuscriptBookScreeningVm(int bookId)
        {
            return new ManuscriptBookScreeningVm(_manuscriptScreeningBL.GetManuscriptBoookScreeningDTO(bookId));
        }

        public ManuscriptBookScreeningVm GetManuscriptBookScreeningDefaultVm()
        {
            ManuscriptBookScreeningDTO manuscriptScreeningDTO = _manuscriptScreeningBL.GetManuscriptBookScreeningDefaultDTO();
            return new ManuscriptBookScreeningVm(manuscriptScreeningDTO);
        }

        public void ValidateManuscriptScreening(IDictionary<string,string> dataErrors, ManuscriptScreeningDTO manuscriptScreeningDTO)
        {
            Entities.Manuscript manuscript = manuscriptScreeningDTO.Manuscript;
            if (manuscript.JournalID == null)
                dataErrors.Add("JournalID", "JournalTitle is required.");
            if (manuscript.ArticleTypeID == null)
                dataErrors.Add("ArticleTypeID", "Article Type is required.");
            if (manuscript.ArticleTitle == null)
                dataErrors.Add("ArticleTitle", "Article Title is required.");
            if (manuscript.StartDate == null)
                dataErrors.Add("StartDate", "Start Date is required.");
            if (manuscript.RoleID == null)
                dataErrors.Add("RoleID", "Role is required.");
            if (manuscript.UserID == null)
                dataErrors.Add("UserMasterID", "System UserID is required.");
            if (manuscript.Crosscheck_iThenticateResultID == null)
                dataErrors.Add("Crosscheck_iThenticateResultID", "Crosscheck iThenticateResult is required.");
            if (manuscript.Highest_iThenticateFromSingleSrc == null)
                dataErrors.Add("Highest_iThenticateFromSingleSrc", "Highest iThenticate(From SingleSource) is required.");
            if (manuscript.English_Lang_QualityID == null)
                dataErrors.Add("English_Lang_QualityID", "English Language Quality is required.");
            if (manuscript.Ethics_ComplianceID == null)
                dataErrors.Add("Ethics_ComplianceID", "Ethics Compliance is required.");
            if (manuscript.InitialSubmissionDate == null)
                dataErrors.Add("InitialSubmissionDate", "Initial Submission Date is required.");
            if (manuscript.CorrespondingAuthor == null)
                dataErrors.Add("CorrespondingAuthor", "Corresponding Author is required.");
            if (manuscript.CorrespondingAuthorEmail == null)
                dataErrors.Add("CorrespondingAuthorEmail", "Corresponding Author Email is required.");
            if (manuscript.CorrespondingAuthorAff == null)
                dataErrors.Add("CorrespondingAuthorAff", "Corresponding Author Aff. is required.");
            if (manuscript.OverallAnalysisID == null)
                dataErrors.Add("OverallAnalysis", "Overall Analysis is required.");
        }

        public bool SaveManuscriptScreeningVM(IDictionary<string,string> dataErrors, ManuscripScreeningVM manuscriptVM)
        {
            ManuscriptScreeningDTO manuscriptScreeningDTO = manuscriptVM.FetchDTO;
            manuscriptScreeningDTO.CurrentUserID = System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            ValidateManuscriptScreening(dataErrors, manuscriptScreeningDTO);
            if (dataErrors.Count == 0)
            {
                _manuscriptScreeningBL.SaveManuscriptScreening(manuscriptScreeningDTO,dataErrors);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
