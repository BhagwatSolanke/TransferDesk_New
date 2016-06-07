using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptAdmin;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.DataContext;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class BookMasterRepository
    {
        private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public BookMasterRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public BookMasterRepository(ManuscriptDBContext context)
        {
            this.context = context;
        }

        public void AddBookMaster(Entities.BookMaster bookMaster)
        {
            bookMaster.CreatedDate = System.DateTime.Now;
            bookMaster.IsActive = true;
            context.BookMaster.Add(bookMaster);
            context.SaveChanges();
        }

        public void UpdateBookMaster(Entities.BookMaster bookMaster)
        {
            bookMaster.ModifiedDate = System.DateTime.Now;
            context.Entry(bookMaster).State = EntityState.Modified;
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



        public bool IsBookTitleAvailable(string bookTitle,int id)
        {
            if (id == 0)
            {
                var result = (from book in context.BookMaster
                              where book.BookTitle == bookTitle.Trim()
                              select book).ToList().Count;
                return (result > 0 ? true : false);
            }
            else
            {
                var result = (from book in context.BookMaster
                              where book.BookTitle == bookTitle.Trim()
                              select book).ToList();
                if (result.Count() == 1)
                {
                    var pkCheck = from book in result
                        where book.ID == id
                        select book;
                    return ((pkCheck.ToList().Count == 1 )? false : true);
                }
                else
                {
                    return result.Count() > 1 ? true : false;
                }
            }
        }

        public BookMaster GetBookInfoById(int id)
        {
            var bookMaster = (from book in context.BookMaster
                where book.ID == id
                select book).FirstOrDefault();
            return bookMaster;
        }

        public List<BookMaster> GetBookMasterDetails()
        {
            var bookMasterDetails = (from bookinfo in context.BookMaster
                select bookinfo).ToList();
            return bookMasterDetails;
        }
    }
}

