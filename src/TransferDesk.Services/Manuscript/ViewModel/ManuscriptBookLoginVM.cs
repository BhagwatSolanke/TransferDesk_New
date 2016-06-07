using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptLogin;
using TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class ManuscriptBookLoginVM
    {
        public ManuscriptBookLoginDTO _msDTO;

        internal ManuscriptBookLoginDTO FetchDTO
        {
            get
            {
                return _msDTO;
            }
        }
        public List<StatusMaster> TaskList { get; set; }
        public List<StatusMaster> ServiceTypes { get; set; }
        public List<BookMaster> BookTitleList { get; set; }
        public List<pr_GetBookLoignedJobs_Result> ManuscriptBookLoginedJobs { get; set; }
        
        public ManuscriptBookLoginVM()
        {
            _msDTO = new ManuscriptBookLoginDTO();
            
        }

        public int ID
        {
            get { return _msDTO.manuscriptBookLogin.ID; }
            set { _msDTO.manuscriptBookLogin.ID = value; }
        }

        [Required(ErrorMessage = "Book Title")]
        public int BookMasterId
        {
            get { return _msDTO.manuscriptBookLogin.BookMasterID; }
            set { _msDTO.manuscriptBookLogin.BookMasterID = value; }
        }

        [Required(ErrorMessage = "Chapter Title")]
        public string ChapterTitle
        {
            get { return _msDTO.manuscriptBookLogin.ChapterTitle; }
            set { _msDTO.manuscriptBookLogin.ChapterTitle = value; }
        }

        [Required(ErrorMessage = "Chapter Number")]
        public string ChapterNumber
        {
            get { return _msDTO.manuscriptBookLogin.ChapterNumber; }
            set { _msDTO.manuscriptBookLogin.ChapterNumber = value; }
        }

        [Required(ErrorMessage = "FTPLink Name")]
        public string FTPLink
        {
            get { return _msDTO.FTPLink; }
            set { _msDTO.FTPLink = value; }
        }

        [Required(ErrorMessage = "FTPLink Name")]
        public string GPUInformation
        {
            get { return _msDTO.GPUInformation; }
            set { _msDTO.GPUInformation = value; }
        }

        [Required(ErrorMessage = "Requester Name")]
        public string RequesterName
        {
            get { return _msDTO.manuscriptBookLogin.RequesterName; }
            set { _msDTO.manuscriptBookLogin.RequesterName = value; }
        }

        public string SpecialInstruction
        {
            get { return _msDTO.manuscriptBookLogin.SpecialInstruction; }
            set { _msDTO.manuscriptBookLogin.SpecialInstruction = value; }
        }
        [Required(ErrorMessage = "Received Date")]
        public DateTime ReceivedDate
        {
            get { return _msDTO.manuscriptBookLogin.ReceivedDate; }
            set { _msDTO.manuscriptBookLogin.ReceivedDate = value; }
        }

        public int PageCount
        {
            get { return _msDTO.manuscriptBookLogin.PageCount; }
            set { _msDTO.manuscriptBookLogin.PageCount = value; }
        }

        public int? StatusMasterTaskID
        {
            get { return _msDTO.manuscriptBookLogin.StatusMasterTaskID; }
            set { _msDTO.manuscriptBookLogin.StatusMasterTaskID = value; }
        }

        [Required(ErrorMessage = "Service Type")]
        public int ServiceTypeID
        {
            get { return _msDTO.manuscriptBookLogin.ServiceTypeID; }
            set { _msDTO.manuscriptBookLogin.ServiceTypeID = value; }
        }

        public int? Revision
        {
            get { return _msDTO.manuscriptBookLogin.Revision; }
            set { _msDTO.manuscriptBookLogin.Revision = value; }
        }

        public string AssociateName
        {
            get { return _msDTO.AssociateName; }
            set { _msDTO.AssociateName = value; }
        }

        public string userId
        {
            get { return _msDTO.userId; }
            set { _msDTO.userId = value; }
        }

        public DateTime? CreatedDate
        {
            get { return _msDTO.manuscriptBookLogin.CreatedDate; }
            set { _msDTO.manuscriptBookLogin.CreatedDate = value; }
        }
        public string CreatedBy
        {
            get { return _msDTO.manuscriptBookLogin.CreatedBy; }
            set { _msDTO.manuscriptBookLogin.CreatedBy = value; }
        }
    }
}

