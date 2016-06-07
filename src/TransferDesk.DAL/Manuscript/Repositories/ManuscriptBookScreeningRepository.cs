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
    public class ManuscriptBookScreeningRepository
    {
        private ManuscriptDBContext context;
        private bool disposed = false;

        public ManuscriptBookScreeningRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public ManuscriptBookScreeningRepository(ManuscriptDBContext context)
        {
            this.context = context;
        }

        public void AddManuscriptBookScreening(Entities.ManuscriptBookScreening manuscriptBookScreening)
        {
            manuscriptBookScreening.CreatedDate = DateTime.Now;
            context.ManuscriptBookScreening.Add(manuscriptBookScreening);
        }

        public void UpdateManuscriptBookScreening(Entities.ManuscriptBookScreening manuscriptBookScreening)
        {
            manuscriptBookScreening.ModifidedDate = DateTime.Now;
            ManuscriptBookScreening existing = context.ManuscriptBookScreening.Find(manuscriptBookScreening.ID);
            ((IObjectContextAdapter)context).ObjectContext.Detach(existing);
            context.Entry(manuscriptBookScreening).State = EntityState.Modified;
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
