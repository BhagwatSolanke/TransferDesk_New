using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RepositoryInterfaces = TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DataContexts = TransferDesk.DAL.Manuscript.DataContext;
using TransferDesk.DAL.Manuscript.DataContext;
namespace TransferDesk.DAL.Manuscript.Repositories
{
    class MSSReviewerMailRepository : IDisposable
    {
          private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public MSSReviewerMailRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public MSSReviewerMailRepository(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public IEnumerable<Entities.MSSReviewerMail> GetReviewerMail()
        {
            return context.MSSReviewerMail.ToList<Entities.MSSReviewerMail>();
        }

        public Entities.MSSReviewerMail GetReviewerMailByID(int id)
        {
            return context.MSSReviewerMail.Find(id);
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
