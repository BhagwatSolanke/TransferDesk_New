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
    class MSSAreaOfExpertise : IDisposable
    {
          private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public MSSAreaOfExpertise(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public MSSAreaOfExpertise(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public IEnumerable<Entities.MSSAreaOfExpertise> GetAreaOfExpertise()
        {
            return context.MSSAreaOfExpertise.ToList<Entities.MSSAreaOfExpertise>();
        }

        public Entities.MSSAreaOfExpertise GetAreaOfExpertiseByID(int id)
        {
            return context.MSSAreaOfExpertise.Find(id);
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
