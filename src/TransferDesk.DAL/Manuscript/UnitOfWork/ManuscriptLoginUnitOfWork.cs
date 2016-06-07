using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Data Access Layer
using Repos = TransferDesk.DAL.Manuscript.Repositories;
using DContext = TransferDesk.DAL.Manuscript.DataContext;

//Contracts
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DTOs = TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.Contracts.Manuscript.DTO;
namespace TransferDesk.DAL.Manuscript.UnitOfWork
{
    public class ManuscriptLoginUnitOfWork:IDisposable
    {

        private Repos.ManuscriptLoginRepository _manuscriptLoginRepository;
        private Repos.ManuscriptLoginDetailsRepository _manuscriptLoginDetailsRepository;
        private Repos.ManuscriptBookLoginRepository _manuscriptBookLoginRepository;
        private Repos.ManuscriptBookLoginDetailsRepository _manuscriptBookLoginDetailsRepository;

        public DTOs.ManuscriptLoginDTO manuscriptLoginDTO { get; set; }
        public DTOs.ManuscriptBookLoginDTO manuscriptBookLoginDTO { get; set; }

        public ManuscriptLoginUnitOfWork(string conString)
        {
            _manuscriptLoginRepository = new Repos.ManuscriptLoginRepository(conString);
            _manuscriptLoginDetailsRepository = new Repos.ManuscriptLoginDetailsRepository(_manuscriptLoginRepository.context);

            _manuscriptBookLoginRepository = new Repos.ManuscriptBookLoginRepository(conString);
            _manuscriptBookLoginDetailsRepository=new Repos.ManuscriptBookLoginDetailsRepository(conString);
        }

        public void SaveManuscriptBookLoginDetails()
        {
            foreach (var item in manuscriptBookLoginDTO.manuscriptBookLoginDetails)
            {
                if (item.Id == 0 || item.Id == null)
                    _manuscriptBookLoginDetailsRepository.AddManuscriptBookLoginDetails(item);
                else
                {
                    _manuscriptBookLoginDetailsRepository.UpdateManuscriptBookLoginDetails(item);
                }

            }
            _manuscriptBookLoginDetailsRepository.SaveChanges();
        }

        public void SaveManuscriptLoginDetails()
        {
            foreach (var item in manuscriptLoginDTO.manuscriptLoginDetails)
            {
                if (item.Id == 0 || item.Id == null)
                      _manuscriptLoginDetailsRepository.AddManuscriptLoginDetails(item);
                else
                {
                    _manuscriptLoginDetailsRepository.UpdateManuscriptLoginDetails(item);
                }
                
            }
            _manuscriptLoginDetailsRepository.SaveChanges();
        }

        public void SaveManuscriptBookLogin(ManuscriptBookLoginDTO manuscriptBookLoginDTO)
        {
            if (manuscriptBookLoginDTO.manuscriptBookLogin.ID==0) // first sent starting from create new then
            {
                manuscriptBookLoginDTO.manuscriptBookLogin.CreatedDate = System.DateTime.Now;
                manuscriptBookLoginDTO.manuscriptBookLogin.CreatedBy = manuscriptBookLoginDTO.userId;
                _manuscriptBookLoginRepository.AddManuscriptBookLogin(manuscriptBookLoginDTO.manuscriptBookLogin);
            }
            else
            {
                manuscriptBookLoginDTO.manuscriptBookLogin.ModifidedDate = System.DateTime.Now;
                manuscriptBookLoginDTO.manuscriptBookLogin.ModifidedBy= manuscriptBookLoginDTO.userId;
                _manuscriptBookLoginRepository.UpdateManuscriptBookLogin(manuscriptBookLoginDTO.manuscriptBookLogin);
            }
            _manuscriptBookLoginRepository.SaveChanges();
        }
        
        public void SaveManuscriptLogin()
        {
            if (manuscriptLoginDTO.manuscriptLogin.CrestId == 0) // first sent starting from create new then
            {
                manuscriptLoginDTO.manuscriptLogin.CreatedDate = System.DateTime.Now;
                manuscriptLoginDTO.manuscriptLogin.CreatedBy = manuscriptLoginDTO.userID;
                _manuscriptLoginRepository.AddManuscriptLogin(manuscriptLoginDTO.manuscriptLogin);
            }
            else
            {
                manuscriptLoginDTO.manuscriptLogin.ModifiedDate = System.DateTime.Now;
                manuscriptLoginDTO.manuscriptLogin.ModifiedBy = manuscriptLoginDTO.userID;
                _manuscriptLoginRepository.UpdateManuscriptLogin(manuscriptLoginDTO.manuscriptLogin);
            }
            _manuscriptLoginRepository.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //todo:check null to check if instance is created
                    _manuscriptLoginRepository.Dispose();
                    _manuscriptLoginDetailsRepository.Dispose();

                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
