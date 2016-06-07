using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    using System.Data.Entity;
    using TransferDesk.Contracts.Manuscript.Entities;
    using TransferDesk.DAL.Manuscript.DataContext;
    public class EmailDetailsRepository : IDisposable
    {
         public ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public EmailDetailsRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public EmailDetailsRepository(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public EmailDetails GetEmailDetail(int id)
        {
            return context.EmailDetails.Find(id);
        }

        public void AddEmailDetail(EmailDetails emailDetail)
        {
            context.EmailDetails.Add(emailDetail);
            context.SaveChanges();
        }

        public void UpdateEmailDetail(EmailDetails emailDetail)
        {
            context.Entry(emailDetail).State = EntityState.Modified;
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
