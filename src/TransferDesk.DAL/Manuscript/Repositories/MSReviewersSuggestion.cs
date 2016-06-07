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
    class MSReviewersSuggestion : IDisposable
    {
          public ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public MSReviewersSuggestion(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public MSReviewersSuggestion(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public IEnumerable<Entities.MSReviewersSuggestion> GetReviewersSuggestion()
        {
            return context.MSReviewersSuggestion.ToList<Entities.MSReviewersSuggestion>();
        }

        public Entities.MSReviewersSuggestion GetReviewersSuggestionByID(int id)
        {
            return context.MSReviewersSuggestion.Find(id);
        }

        public int? AddMSReviewersSuggestion(Entities.MSReviewersSuggestion msReviewersSuggestion)
        {
            msReviewersSuggestion.CreatedDate = System.DateTime.Now;
            msReviewersSuggestion.IsActive = true;
            context.MSReviewersSuggestion.Add(msReviewersSuggestion);
            context.SaveChanges();
            return msReviewersSuggestion.ID;
        }

        public void UpdateMSReviewersSuggestion(Entities.MSReviewersSuggestion msReviewersSuggestion)
        {
            msReviewersSuggestion.QualitySubmissionDate = System.DateTime.Now;
            context.Entry(msReviewersSuggestion).State = EntityState.Modified;
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

