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
    class MSReviewerInfo
    {
          private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public MSReviewerInfo(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public MSReviewerInfo(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public IEnumerable<Entities.MSReviewersSuggestionInfo> GetReviewerInfo()
        {
            return context.MSReviewersSuggestionInfo.ToList<Entities.MSReviewersSuggestionInfo>();
        }

        public Entities.MSReviewersSuggestionInfo GetReviewerInfoByID(int id)
        {
            return context.MSReviewersSuggestionInfo.Find(id);
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



        internal void UnAssignReviewer(int reviewerInfoID, int? msReviewersSuggestionID)
        {
            var reviewerInfo = new Entities.MSReviewersSuggestionInfo();
            reviewerInfo = context.MSReviewersSuggestionInfo.Where(x => x.ID == reviewerInfoID).FirstOrDefault();
            reviewerInfo.IsActive = false;
            reviewerInfo.IsAssociateFinalSubmit = false;
            context.Entry(reviewerInfo).State = EntityState.Modified;  
            context.SaveChanges();
        }
    }
}
