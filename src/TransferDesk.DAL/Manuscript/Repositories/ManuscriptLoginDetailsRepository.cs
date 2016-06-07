using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Repositories;
using DataContexts = TransferDesk.DAL.Manuscript.DataContext;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class ManuscriptLoginDetailsRepository:IManuscriptLoginDetails 
    {
        public DataContexts.ManuscriptDBContext context;
        private bool disposed = false;

        public ManuscriptLoginDetailsRepository(string conString)
        {
            this.context = new DataContexts.ManuscriptDBContext(conString);
        }
        public ManuscriptLoginDetailsRepository(DataContexts.ManuscriptDBContext context)
        {
            this.context = context;
        }
        public void AddManuscriptLoginDetails(Entities.ManuscriptLoginDetails manuscriptLoginDetails)
        {
            manuscriptLoginDetails.CreatedDate = System.DateTime.Now;
            context.ManuscriptLoginDetails.Add(manuscriptLoginDetails);
        }

        public void UpdateManuscriptLoginDetails(Entities.ManuscriptLoginDetails manuscriptLoginDetails)
        {
            manuscriptLoginDetails.ModifiedDate= System.DateTime.Now;
            //context.Entry(manuscriptLoginDetails).State = EntityState.Modified;
            Entities.ManuscriptLoginDetails existing = context.ManuscriptLoginDetails.Find(manuscriptLoginDetails.Id);
            ((IObjectContextAdapter)context).ObjectContext.Detach(existing);
            context.Entry(manuscriptLoginDetails).State = EntityState.Modified;
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

        public int GetOpenManuscriptCount(int crestId)
        {
            var serviceTypeStatusId = (from q in context.ManuscriptLoginDetails
                                       where q.CrestId == crestId && q.JobStatusId == 7
                                       select q.ServiceTypeStatusId).Count();
            return serviceTypeStatusId;
        }

    }
}
