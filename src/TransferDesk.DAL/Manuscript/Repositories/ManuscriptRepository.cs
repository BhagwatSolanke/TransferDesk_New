using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;

using RepositoryInterfaces = TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DataContexts = TransferDesk.DAL.Manuscript.DataContext;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class ManuscriptRepository : RepositoryInterfaces.IManuscriptRepository, IDisposable
    {
        public DataContexts.ManuscriptDBContext manuscriptDataContext;

        public ManuscriptRepository(string conString)
        {
            this.manuscriptDataContext = new DataContexts.ManuscriptDBContext(conString);
        }
      
        public int? AddManuscript(Entities.Manuscript manuscript)
        {
            manuscript.ModifiedDateTime = System.DateTime.Now;
            manuscript.StartDate = System.DateTime.Now;
            manuscript.Status = 1;
            manuscriptDataContext.Manuscripts.Add(manuscript);
            manuscriptDataContext.SaveChanges();
            return manuscript.ID;
        }

        public void UpdateManuscript(Entities.Manuscript manuscript)
        {
            manuscript.ModifiedDateTime = System.DateTime.Now;
            manuscriptDataContext.Entry(manuscript).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            manuscriptDataContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    manuscriptDataContext.Dispose();
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
