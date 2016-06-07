using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;

using TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

using TransferDesk.DAL.Manuscript.DataContext;


namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class OtherAuthorsRepository: IDisposable
    {
        private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public OtherAuthorsRepository(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public void AddOtherAuthor(Entities.OtherAuthor otherAuthor)
        {
            //todo: shift this system updated system fields to unitofwork 
            otherAuthor.ModifiedDateTime = System.DateTime.Now;
            otherAuthor.Status = 1;//todo:later status will have status enums
            context.OtherAuthors.Add(otherAuthor);
            context.SaveChanges();
        }

        public void UpdateOtherAuthor(Entities.OtherAuthor otherAuthor)
        {
            otherAuthor.ModifiedDateTime = System.DateTime.Now;
            otherAuthor.Status = 2;
           context.Entry(otherAuthor).State = EntityState.Modified;
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
