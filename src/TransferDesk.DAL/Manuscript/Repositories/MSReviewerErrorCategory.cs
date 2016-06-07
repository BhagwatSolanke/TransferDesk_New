using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RepositoryInterfaces = TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DataContexts = TransferDesk.DAL.Manuscript.DataContext;
using TransferDesk.DAL.Manuscript.DataContext;
using System.Data.Entity;
namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class MSReviewerErrorCategory : IDisposable
    {
          private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public MSReviewerErrorCategory(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public MSReviewerErrorCategory(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public IEnumerable<Entities.MSReviewerErrorCategory> GetMSReviewerErrorCategory()
        {
            return context.MSReviewerErrorCategory.ToList<Entities.MSReviewerErrorCategory>();
        }

        public Entities.MSReviewerErrorCategory GetMSReviewerErrorCategoryByID(int id)
        {
            return context.MSReviewerErrorCategory.Find(id);
        }

        public void AddMSReviewerErrorCategory(Entities.MSReviewerErrorCategory msReviewerErrorCategory)
        {
            context.MSReviewerErrorCategory.Add(msReviewerErrorCategory);
        }

        public void UpdateMSReviewerErrorCategory(Entities.MSReviewerErrorCategory msReviewerErrorCategory)
        {
            context.Entry(msReviewerErrorCategory).State = EntityState.Modified;
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

