using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.DataContext;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptAdmin;
using TransferDesk.Contracts.Manuscript.Entities;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class JournalArticleTypeRepository : IDisposable
    {
        private ManuscriptDBContext context;

        private bool disposed = false;

        public JournalArticleTypeRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public IEnumerable<Journal> GetJournalTitleByJournalID(string JournalID)
        {
            var journals = from journal in context.Journals
                           select journal;
            return journals.ToList<Entities.Journal>();

        }

        public IEnumerable<Entities.JournalArticleTypes> getData()
        {
            return context.JournalArticleTypes.ToList<Entities.JournalArticleTypes>();

            //  return context.JournalArticleTypes.ToList<Entities.JournalArticleTypes>();
        }

        public JournalArticleTypeRepository(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public int JournalIDvalue(string JournalName)
        {
            var journalid = from a in context.Journals
                            where a.JournalTitle.ToLower().Trim() == JournalName.ToLower().Trim()
                            select a;
            foreach (var journalidvalue in journalid)
            {
                return journalidvalue.ID;
            }

            return 0;
        }

        public bool AddJournalArticleData(int journalidvalue, int articleidvalue)
        {
            try
            {
                var _JournalArticleAdd = new JournalArticleTypes();
                _JournalArticleAdd.JournalID = journalidvalue;
                _JournalArticleAdd.ArticleTypeID = articleidvalue;
                _JournalArticleAdd.Status = 1;
                _JournalArticleAdd.IsActive = true;
                _JournalArticleAdd.ModifiedDateTime = System.DateTime.Now;
                AddJournalArticleType(_JournalArticleAdd);
                SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;

        }

        public void UpdateJournalArticle(Entities.JournalArticleTypes journalarticleType)
        {
            context.Entry(journalarticleType).State = EntityState.Modified;
        }

        public void AddArticleType(Entities.ArticleType articleType)
        {
            context.ArticleTypes.Add(articleType);
        }


        public bool AddArticleData(string articlename)
        {
            try
            {
                var _ArticleName = new ArticleType();
                _ArticleName.ArticleTypeName = articlename;
                _ArticleName.Status = 1;
                _ArticleName.IsActive = true;
                _ArticleName.ModifiedDateTime = System.DateTime.Now;
                AddArticleType(_ArticleName);
                SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;

        }

        public void AddJournalArticleType(Entities.JournalArticleTypes journalarticle)
        {
            context.JournalArticleTypes.Add(journalarticle);
        }

        public IEnumerable<pr_GetJournalArticleDetails_Result> GetJournalArticleDetails(int journalid)
        {
            try
            {
                //   Nullable<int> value = Convert.ToInt32(journalid);
                //   var selectedjouralid = value.HasValue ?

                //       new SqlParameter("journalid", journalid) :
                // new SqlParameter("journalid", typeof(global::System.Int32));

                //   IEnumerable<pr_GetJournalArticleDetails_Result> journalarticleDetails =
                //this.context.Database.SqlQuery<pr_GetJournalArticleDetails_Result>("exec pr_GetJournalArticleDetails @journalid", selectedjouralid).ToList();
                //   return journalarticleDetails;

                Nullable<int> value = Convert.ToInt32(journalid);
                var selectedjouralid = value.HasValue ?

                    new SqlParameter("journalid", journalid) :
              new SqlParameter("journalid", typeof(global::System.Int32));
                IEnumerable<pr_GetJournalArticleDetails_Result> empDetails = this.context.Database.SqlQuery
                                                                                  <pr_GetJournalArticleDetails_Result>("exec pr_GetJournalArticleDetails @journalid", selectedjouralid).ToList();
                return empDetails;
            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }
        }

        public int articleID(string articleName)
        {
            var articleID = from a in context.ArticleTypes
                            where a.ArticleTypeName.ToLower().Trim() == articleName.ToLower().Trim()
                            select a;
            foreach (var artname in articleID)
            {
                return artname.ID;
            }

            return 0;
        }

        public bool IsArticleTypeAvailable(string articlename, int journaliddata)
        {
            var result = from q in context.ArticleTypes
                         where q.ArticleTypeName.ToLower().Trim() == articlename.ToLower().Trim()
                         select q;
            if (result.ToList().Count() > 0)
            {
                var collectdata = from j in result
                                  select j.ID;
                foreach (var artid in collectdata)
                {
                    var journaldata = (from j in context.JournalArticleTypes
                                       where j.ArticleTypeID == artid && j.JournalID == journaliddata
                                       select j);
                    if (journaldata.ToList().Count() > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            return false;

        }

        public bool IsArticleNameAvailable(string articlename)
        {
            var result = from q in context.ArticleTypes
                         where q.ArticleTypeName.ToLower().Trim() == articlename.ToLower().Trim()
                         select q;
            if (result.ToList().Count() > 0)
            {
                return true;
            }
            else
                return false;
        }

        public bool IsJournalArticleTypeAvailable(int journalid, int artid, string artname, bool isactivedata)
        {
            var result = from q in context.JournalArticleTypes
                         where q.ArticleTypeID == artid && q.IsActive == isactivedata && q.JournalID == journalid
                         select q;
            if (result.ToList().Count() > 0)
            {
                var matchdata = (from m in context.ArticleTypes
                                 where m.ArticleTypeName.ToLower() == artname.ToLower() && m.ID == artid
                                 select m).Count();
                if (matchdata > 0)
                    return true;
                else
                    return false;
            }
            return false;
            //else
            //    return false;



            //var artdata = from q in context.ArticleTypes
            //              where q.ArticleTypeName.ToLower() == artname.ToLower()
            //              select q;
            //if (artdata.ToList().Count() == 1)
            //{
            //    var pkCheck = from q in artdata
            //                  where q.ID == artid
            //                  select q;
            //    if (pkCheck.ToList().Count() == 1)
            //        return false;
            //    else
            //        return true;
            //}

            //    var userRoles = from q in context.Journals
            //                    where q.JournalTitle.ToLower() == Title.ToLower()
            //                    select q;
            //    if (userRoles.ToList().Count() == 1)
            //    {
            //        var pkCheck = from userRole in userRoles
            //                      where userRole.ID == id
            //                      select userRole;
            //        if (pkCheck.ToList().Count() == 1)
            //            return false;
            //        else
            //            return true;
            //    }
            //    else if (userRoles.ToList().Count() > 1)
            //        return true;
            //    else
            //        return false;
            //var result = from q in context.JournalArticleTypes
            //             where q.ArticleTypeID == artid
            //             select q;
            //if (result.ToList().Count() > 0)
            //    return true;
            //else
            //    return false;

        }

        public List<Entities.ArticleType> GetArticleTypeList()
        {
            return context.ArticleTypes.ToList();
        }

        public void UpdateArticleType(Entities.ArticleType articleType)
        {
            context.Entry(articleType).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public bool IsAdmin(string userId)
        {
            var count = (from userAdmin in context.UserAdmin
                         where userAdmin.UserID == userId
                         select userAdmin).Count();
            if (count > 0)
                return true;
            else
                return false;
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
