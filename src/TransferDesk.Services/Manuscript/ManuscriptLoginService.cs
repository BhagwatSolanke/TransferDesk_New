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
using System.Configuration;
namespace TransferDesk.Services.Manuscript
{
    public class ManuscriptLoginService
    {
        public String conString { get; set; }

        public ManuscriptLoginBL _manuscriptLoginBL { get; set; }
       // Entities.ManuscriptLogin manuscriptLogin { get; set; }
        Entities.ManuscriptLoginDetails manuscriptLoginDetails { get; set; }
        ManuscriptLoginDTO manuscriptLoginDTO { get; set; }
        ManuscriptBookLoginDTO manuscriptBookLoginDTO { get; set; }

        public ManuscriptLoginService(String conString)
        {
            _manuscriptLoginBL = new ManuscriptLoginBL(conString);
        }

        public bool SaveManuscriptBookLoginVM(IDictionary<string, string> dataErrors,ManuscriptBookLoginVM manuscriptBookLoginVM, Entities.ManuscriptBookLogin manuscriptBookLogin)
        {
            //if validation required add here
            if (dataErrors.Count == 0)
            {
                manuscriptBookLoginDTO = new ManuscriptBookLoginDTO();
                manuscriptBookLoginDTO = manuscriptBookLoginVM.FetchDTO;
                
                if (_manuscriptLoginBL.SaveManuscriptBookLogin(manuscriptBookLoginDTO, dataErrors))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool SaveManuscriptLoginVM(IDictionary<string, string> dataErrors, ManuscriptLoginVM manuscriptLoginVM,Entities.ManuscriptLogin manuscriptLogin)
        {

            ValidateManuscriptLogin(dataErrors, manuscriptLoginVM);
            if (dataErrors.Count == 0)
            {
                manuscriptLoginDTO = new ManuscriptLoginDTO();
                //manuscriptLogin = new Entities.ManuscriptLogin();

                //ManuscriptLogin entity data fill up
                if (manuscriptLoginVM.CrestId == null || manuscriptLoginVM.CrestId==0)
                {
                    manuscriptLogin.CrestId = Convert.ToInt32(manuscriptLoginVM.CrestId);
                    //manuscriptLogin.ManuscriptFilePath = manuscriptLoginVM.ManuscriptFilePath;
                }    

                manuscriptLogin.ArticleTitle = manuscriptLoginVM.ArticleTitle;
                manuscriptLogin.ArticleTypeId = manuscriptLoginVM.ArticleTypeID;
                manuscriptLogin.InitialSubmissionDate = manuscriptLoginVM.InitialSubmissionDate;
                manuscriptLogin.SpecialInstruction = manuscriptLoginVM.SpecialInstruction;
                manuscriptLogin.SectionId = manuscriptLoginVM.SectionID;
                manuscriptLogin.ReceivedDate = manuscriptLoginVM.ReceivedDate;
                manuscriptLogin.TaskID = manuscriptLoginVM.TaskID;
                manuscriptLogin.JournalId = (manuscriptLoginVM.JournalID == 0) ? manuscriptLogin.JournalId : manuscriptLoginVM.JournalID;
                manuscriptLogin.ServiceTypeStatusId = manuscriptLoginVM.ServiceTypeID;
                manuscriptLogin.MSID = manuscriptLoginVM.MSID;
                //assign manuscriptlogin enitity object to DTO
                manuscriptLoginDTO.manuscriptLogin = manuscriptLogin;
                manuscriptLoginDTO.AssociateName = manuscriptLoginVM.Associate;
                manuscriptLoginDTO.IsRevision = manuscriptLoginVM.IsRevision;
                if (_manuscriptLoginBL.SaveManuscriptLogin(manuscriptLoginDTO, dataErrors))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private void ValidateManuscriptLogin(IDictionary<string, string> dataErrors, ManuscriptLoginVM manuscriptLoginVM)
        {
            if (manuscriptLoginVM.JournalID == null)
                dataErrors.Add("JournalID", "JournalTitle is required.");
            if (manuscriptLoginVM.ArticleTypeID == null)
                dataErrors.Add("ArticleTypeID", "Article Type is required.");
            if (manuscriptLoginVM.ArticleTitle == null)
                dataErrors.Add("ArticleTitle", "Article Title is required.");
            if (manuscriptLoginVM.InitialSubmissionDate == null)
                dataErrors.Add("InitialSubmissionDate", "Initial Submission Date is required.");
            if (manuscriptLoginVM.MSID == null)
                dataErrors.Add("MSID", "Manuscript Number is required.");
            if (manuscriptLoginVM.ServiceTypeID == null)
                dataErrors.Add("ServiceTypeID", "Service Type is required.");
        }
    }
}
