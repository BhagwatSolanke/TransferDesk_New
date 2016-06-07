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
    public class ManuscriptScreeningUnitOfWork : IDisposable
    {
        private Repos.ManuscriptRepository _manuscriptRepository;
        private Repos.OtherAuthorsRepository _otherAuthorsRepository;
        private Repos.ManuscriptErrorCategoryRepository _manuscriptErrorCategoryRepository;
        private Repos.ErrorCategoryRepository _errorCategoryRepository;

        public DTOs.ManuscriptScreeningDTO manuscriptScreeningDTO { get; set; }

        public ManuscriptScreeningUnitOfWork(string conString)
        {
            _manuscriptRepository = new Repos.ManuscriptRepository(conString);
            _otherAuthorsRepository = new Repos.OtherAuthorsRepository(_manuscriptRepository.manuscriptDataContext);
            _errorCategoryRepository = new Repos.ErrorCategoryRepository(_manuscriptRepository.manuscriptDataContext);
            _manuscriptErrorCategoryRepository = new Repos.ManuscriptErrorCategoryRepository(_manuscriptRepository.manuscriptDataContext);
        }


        //private void SaveOtherAuthors()
        //{
        //    if (manuscriptScreeningDTO.OtherAuthors != null)
        //    {
        //        foreach (Entities.OtherAuthor otherAuthor in manuscriptScreeningDTO.OtherAuthors)
        //        {
        //            if (otherAuthor.ID == null || otherAuthor.ID == 0)
        //            {
        //                otherAuthor.ManuscriptID = manuscriptScreeningDTO.Manuscript.ID;
        //                otherAuthor.MSID = manuscriptScreeningDTO.Manuscript.MSID;
        //                _otherAuthorsRepository.AddOtherAuthor(otherAuthor);
        //            }
        //            else
        //            {
        //                otherAuthor.ManuscriptID = manuscriptScreeningDTO.Manuscript.ID;
        //                otherAuthor.MSID = manuscriptScreeningDTO.Manuscript.MSID;
        //                _otherAuthorsRepository.UpdateOtherAuthor(otherAuthor);
        //            }
        //        }
        //    }
        //}

        private void SaveOtherAuthors()
        {
            if (manuscriptScreeningDTO.OtherAuthors != null)
            {
                foreach (Entities.OtherAuthor otherAuthor in manuscriptScreeningDTO.OtherAuthors)
                {
                    if (otherAuthor.ID == null || otherAuthor.ID == 0)
                    {
                        otherAuthor.ManuscriptID = manuscriptScreeningDTO.Manuscript.ID;
                        otherAuthor.MSID = manuscriptScreeningDTO.Manuscript.MSID;
                        if (otherAuthor.Affillation == null && otherAuthor.AuthorName == null)
                        {
                            otherAuthor.Affillation = " ";
                            otherAuthor.AuthorName = " ";
                        }
                        if (!string.IsNullOrEmpty(otherAuthor.Affillation) &&
                            !string.IsNullOrEmpty(otherAuthor.AuthorName))
                        {
                            _otherAuthorsRepository.AddOtherAuthor(otherAuthor);
                        }
                    }
                    else
                    {
                        otherAuthor.ManuscriptID = manuscriptScreeningDTO.Manuscript.ID;
                        otherAuthor.MSID = manuscriptScreeningDTO.Manuscript.MSID;
                        if (otherAuthor.Affillation == null && otherAuthor.AuthorName == null)
                        {
                            otherAuthor.Affillation = " ";
                            otherAuthor.AuthorName = " ";
                        }
                        if (!string.IsNullOrEmpty(otherAuthor.Affillation) &&
                            !string.IsNullOrEmpty(otherAuthor.AuthorName))
                        {
                            _otherAuthorsRepository.UpdateOtherAuthor(otherAuthor);
                        }

                    }
                }
            }
        }

        private void SaveManuscriptErrorCategories()
        {
            if (manuscriptScreeningDTO.manuscriptErrorCategoryList != null)
            {
                foreach (Entities.ManuscriptErrorCategory manuscriptErrorCategory in manuscriptScreeningDTO.manuscriptErrorCategoryList)
                {
                    //viewmodel is defined and sent so sure that it is 0(or value) and it is not null
                    if (manuscriptErrorCategory.ID == 0)
                    {
                        manuscriptErrorCategory.ManuscriptID = manuscriptScreeningDTO.Manuscript.ID;
                        manuscriptErrorCategory.MSID = manuscriptScreeningDTO.Manuscript.MSID;
                        _manuscriptErrorCategoryRepository.AddManuscriptErrorCategory(manuscriptErrorCategory);
                    }
                    else
                    {
                        manuscriptErrorCategory.ManuscriptID = manuscriptScreeningDTO.Manuscript.ID;
                        manuscriptErrorCategory.MSID = manuscriptScreeningDTO.Manuscript.MSID;
                        _manuscriptErrorCategoryRepository.UpdateManuscriptErrorCategory(manuscriptErrorCategory);
                    }
                }
            }
        }

        public void SaveManuscriptScreening()
        {
            SaveManuscript();
            SaveOtherAuthors();
            SaveManuscriptErrorCategories();
        }

        private void SaveManuscript()
        {
            if (manuscriptScreeningDTO.Manuscript.ID == 0 ) // first sent starting from create new then
            {
               _manuscriptRepository.AddManuscript(manuscriptScreeningDTO.Manuscript);
            }
            else
            {
                _manuscriptRepository.UpdateManuscript(manuscriptScreeningDTO.Manuscript);
            }
        }

        public void SaveChanges()
        {
            //loop to update ids from first manuscript Save and finally save context
            _manuscriptRepository.SaveChanges();
        }

        //dispose section below

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {             
                    //todo:check null to check if instance is created
                    _otherAuthorsRepository.Dispose();
                    _errorCategoryRepository.Dispose();
                    _manuscriptErrorCategoryRepository.Dispose();
                    _manuscriptRepository.Dispose();

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
