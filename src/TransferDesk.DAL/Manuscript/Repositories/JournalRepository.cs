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
    public class JournalRepository
    {
        private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public JournalRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public JournalRepository(ManuscriptDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<Entities.Journal> GetJournals()
        {
            return context.Journals.OrderBy(o => o.JournalTitle).ToList<Entities.Journal>();
        }

        public bool IsJournalStatusAvailable(string journalname, bool status)
        {
            try
            {
                var result = from q in context.Journals
                             where (q.JournalTitle.ToLower().Trim().Equals(journalname.ToLower().Trim()) && q.IsActive.Equals(status))
                             select q;
                int count = result.ToList().Count();
                if (count > 0)
                    return true;
                else
                    return false;

            }
            catch
            {
                return false;
            }
            finally
            {
            }

        }

        public bool IsJournalTitleAvailable(string journalname)
        {
            try
            {
                var journalTitleName = journalname != null ?
                 new SqlParameter("journalName", journalname) :
                 new SqlParameter("journalName", typeof(global::System.String));

                IEnumerable<pr_GetJournalNameDetails> journalName1 = this.context.Database.SqlQuery
                                                                                 <pr_GetJournalNameDetails>("exec pr_GetJournalName @journalName", journalTitleName).ToList();

                var journalCount = (from q in journalName1
                                    where q.JournalTitle.Trim().ToLower().Replace(System.Environment.NewLine, string.Empty) == journalname.Trim().ToLower()
                                    select q).ToList();

                if (journalCount.Count != 0)

                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool IsJournalArticleAvailable(string title)
        {
            var result = from q in context.Journals
                         where q.JournalTitle.ToLower().Trim().Contains(title.ToLower().Trim())
                         select q;
            int count = result.ToList().Count();
            if (count > 0)
                return true;
            else
                return false;
        }

        public bool IsJournalTitleStatusAvailable(string title, bool active, int id)
        {
            if (id == 0)
            {
                var count = (from q in context.Journals
                             where q.ID == id && q.JournalTitle.Trim().ToLower() == title.Trim().ToLower()
                             select q).Count();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                var userRoles = from q in context.Journals
                                where q.JournalTitle.Trim().ToLower() == title.Trim().ToLower()
                                select q;
                if (userRoles.ToList().Count() == 1)
                {
                    var pkCheck = from userRole in userRoles
                                  where userRole.ID == id
                                  select userRole;
                    if (pkCheck.ToList().Count() == 1)
                        return false;
                    else
                        return true;
                }
                else if (userRoles.ToList().Count() > 1)
                    return true;
                else
                    return false;
            }
        }

        public bool IsJournalIDTitleStatusAvailable(string title, bool active, int id)
        {
            var result = from q in context.Journals
                         where (q.ID.Equals(id))
                         select q;
            int count = result.ToList().Count();
            if (count > 0)
                return true;
            else
                return false;
        }

        public bool IsJournalAvailable(int id)
        {
            var result = from q in context.Journals
                         where q.ID == id
                         select q;
            int count = result.ToList().Count();
            if (count > 0)
                return true;
            else
                return false;
        }

        public void AddJournal(Entities.Journal journal)
        {
            context.Journals.Add(journal);
        }

        public void UpdateJournal(Entities.Journal journal)
        {
            Journal existing = context.Journals.Find(journal.ID);
            ((IObjectContextAdapter)context).ObjectContext.Detach(existing);
            context.Entry(journal).State = EntityState.Modified;
        }

        public bool IsAdmin(string userId)
        {
            var count = (from userAdmin in context.UserAdmin
                         where userAdmin.UserID == userId && (userAdmin.IsAdmin.Equals(true))
                         select userAdmin).Count();
            if (count > 0)
                return true;
            else
                return false;
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
