using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptAdmin;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.DataContext;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class ManuscriptBookLoginRepository
    {
         private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public ManuscriptBookLoginRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public ManuscriptBookLoginRepository(ManuscriptDBContext context)
        {
            this.context = context;
        }

        public void AddManuscriptBookLogin(Entities.ManuscriptBookLogin manuscriptBookLogin)
        {
            manuscriptBookLogin.CreatedDate = DateTime.Now;
            context.ManuscriptBookLogin.Add(manuscriptBookLogin);
        }

        public void UpdateManuscriptBookLogin(Entities.ManuscriptBookLogin manuscriptBookLogin)
        {
            manuscriptBookLogin.ModifidedDate = DateTime.Now;
            ManuscriptBookLogin existing = context.ManuscriptBookLogin.Find(manuscriptBookLogin.ID);
            ((IObjectContextAdapter)context).ObjectContext.Detach(existing);
            context.Entry(manuscriptBookLogin).State = EntityState.Modified;
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
