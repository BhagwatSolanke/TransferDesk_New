using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.DAL.Manuscript.DataContext;
using System.Data.Entity;

using TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class ErrorCategoryRepository : IDisposable
    {
          private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public ErrorCategoryRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public ErrorCategoryRepository(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public void AddErrorCategory(Entities.ErrorCategory errorCategory)
        {
            context.ErrorCategories.Add(errorCategory);
        }

        public void UpdateErrorCategory(Entities.ErrorCategory errorCategory)
        {
           context.Entry(errorCategory).State = EntityState.Modified;
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
