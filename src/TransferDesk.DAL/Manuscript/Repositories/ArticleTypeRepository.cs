using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.DataContext;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class ArticleTypeRepository
    {
          private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public ArticleTypeRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public ArticleTypeRepository(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public void AddArticleType(Entities.ArticleType articleType)
        {
            context.ArticleTypes.Add(articleType);
        }

        public void UpdateArticleType(Entities.ArticleType articleType)
        {
            context.Entry(articleType).State = EntityState.Modified;
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
