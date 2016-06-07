using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

using TransferDesk.DAL.Manuscript.DataContext;
using System.Data.Entity;

namespace TransferDesk.DAL.Manuscript.Repositories
{
   public class ManuscriptErrorCategoryRepository
    {
        private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public ManuscriptErrorCategoryRepository(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        //public List<Entities.ManuscriptErrorCategory> GetManuscriptErrorCategories()
        //{
        //    return context.ManuscriptErrorCategory.ToList<Entities.ManuscriptErrorCategory>();
        //}

        //public List<Entities.ManuscriptErrorCategory> GetManuscriptErrorCategoryList(int manuscriptID)
        //{
        //    var ManuscriptErrorCategoryList = from q in context.ManuscriptErrorCategory
        //                                      where q.ManuscriptID == manuscriptID
        //                                      select q;
        //        return ManuscriptErrorCategoryList.ToList();
        //}

        //public Entities.ManuscriptErrorCategory GetManuscriptErrorCategory(int id)
        //{
        //    return context.ManuscriptErrorCategory.Find(id);
        //}

        public void AddManuscriptErrorCategory(Entities.ManuscriptErrorCategory manuscriptErrorCategory)
        {
            manuscriptErrorCategory.ModifiedDateTime = System.DateTime.Now;
            manuscriptErrorCategory.Status = 1;
           context.ManuscriptErrorCategory.Add(manuscriptErrorCategory);
           context.SaveChanges();
        }

        public void UpdateManuscriptErrorCategory(Entities.ManuscriptErrorCategory manuscriptErrorCategory)
        {
            manuscriptErrorCategory.ModifiedDateTime = System.DateTime.Now;
            manuscriptErrorCategory.Status = 2;
           context.Entry(manuscriptErrorCategory).State = EntityState.Modified;
           context.SaveChanges();
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
