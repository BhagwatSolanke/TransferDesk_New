using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.DataContext;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
namespace TransferDesk.DAL.Manuscript.Repositories
{

    public class ManuscriptLoginRepository:IManuscriptLogin
    {
        public ManuscriptDBContext context;
        //dispose calls
        private bool disposed = false;

        public ManuscriptLoginRepository(ManuscriptDBContext manuscriptDBContext)
        {
            this.context = manuscriptDBContext;
        }
        public ManuscriptLoginRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }
        public void AddManuscriptLogin(Entities.ManuscriptLogin manuscriptLogin)
        {
            context.ManuscriptLogin.Add(manuscriptLogin);
        }

        public void UpdateManuscriptLogin(Entities.ManuscriptLogin manuscriptLogin)
        {
            context.Entry(manuscriptLogin).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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
