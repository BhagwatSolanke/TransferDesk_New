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
using System.Text.RegularExpressions;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class JournalSectionTypeRepository : IDisposable
    {
        private ManuscriptDBContext context;

        //dispose calls
        private bool disposed = false;

        public JournalSectionTypeRepository(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        //GetJournalTitleByJournalID
        public IEnumerable<Journal> GetJournalTitleByJournalID(string JournalID)
        {
            var journals = from journal in context.Journals
                           select journal;
            return journals.ToList<Entities.Journal>();

        }

        public IEnumerable<Entities.JournalSections> getData()
        {
            return context.JournalSecions.ToList<Entities.JournalSections>();
        }

        public JournalSectionTypeRepository(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public void AddSection(Entities.Section sectionType)
        {
            context.Sections.Add(sectionType);
        }
        //AddJournalArticleType
        public void AddJournalSectionType(Entities.JournalSections journalsectiontype)
        {
            context.JournalSecions.Add(journalsectiontype);
        }

        public IEnumerable<pr_GetJournalSectionDetails_Result> GetJournalSectionDetails(int journalid)
        {
            try
            {
                Nullable<int> value = Convert.ToInt32(journalid);
                var selectedjouralid = value.HasValue ?

                    new SqlParameter("journalid", journalid) :
              new SqlParameter("journalid", typeof(global::System.Int32));

                IEnumerable<pr_GetJournalSectionDetails_Result> journalsectionDetails =
             this.context.Database.SqlQuery<pr_GetJournalSectionDetails_Result>("exec pr_GetJournalSectionDetails @journalid", selectedjouralid).ToList();
                return journalsectionDetails;

            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }

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

        public int sectionID(string sectionName)
        {

            var sectionID = from a in context.Sections
                            where a.SectionName.ToLower().Trim() == sectionName.ToLower().Trim()
                            select a;
            foreach (var sectionname in sectionID)
            {
                return sectionname.ID;
            }

            return 0;
        }
        //IsArticleTypeAvailable
        public bool IsSectionTypeAvailable(string sectionname, int journaliddata)
        {
            var result = from q in context.Sections
                         where q.SectionName.ToLower().Trim() == sectionname.ToLower().Trim()
                         select q;
            if (result.ToList().Count() > 0)
            {
                var collectdata = from j in result
                                  select j.ID;
                foreach (var sectionid in collectdata)
                {
                    var journaldata = (from j in context.JournalSecions
                                       where j.SectionID == sectionid && j.JournalID == journaliddata
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

        //IsArticleNameAvailable
        public bool IsSectionNameAvailable(string sectionname)
        {
            var result = from q in context.Sections
                         where q.SectionName.ToLower().Trim() == sectionname.ToLower().Trim()
                         select q;
            if (result.ToList().Count() > 0)
            {
                return true;
            }
            else
                return false;

        }

        //IsJournalArticleTypeAvailable
        public bool IsJournalSectionTypeAvailable(int journalid, int sectionid, string sectionname, bool isactivedata)
        {
            var result = from q in context.JournalSecions
                         where q.SectionID == sectionid && q.IsActive.Equals(isactivedata) && q.JournalID == journalid
                         select q;
            if (result.ToList().Count() > 0)
            {
                var matchdata = (from m in context.Sections
                                 where m.SectionName.ToLower() == sectionname.ToLower() && m.ID == sectionid
                                 select m).Count();
                if (matchdata > 0)
                    return true;
                else
                    return false;
            }
            return false;

        }

        public bool AddSectionData(string sectionname)
        {
            try
            {
                var _SectionName = new Section();
                _SectionName.SectionName = sectionname;
                _SectionName.Status = 1;
                _SectionName.IsActive = true;
                _SectionName.ModifiedDateTime = System.DateTime.Now;
                AddSection(_SectionName);
                SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;

        }

        public bool AddJournalSectionData(int journalidvalue, int sectionidvalue)
        {
            try
            {
                var _JournalSectionAdd = new JournalSections();
                _JournalSectionAdd.JournalID = journalidvalue;
                _JournalSectionAdd.SectionID = sectionidvalue;
                _JournalSectionAdd.Status = 1;
                _JournalSectionAdd.IsActive = true;
                _JournalSectionAdd.ModifiedDateTime = System.DateTime.Now;
                AddJournalSectionType(_JournalSectionAdd);
                SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;

        }

        public List<Entities.Section> GetSectionList()
        {
            return context.Sections.ToList();
        }

        public void UpdateSection(Entities.Section sectionType)
        {
            context.Entry(sectionType).State = EntityState.Modified;
        }

        public void UpdateJournalSection(Entities.JournalSections journalsectionType)
        {
            context.Entry(journalsectionType).State = EntityState.Modified;
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
