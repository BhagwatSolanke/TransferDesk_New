using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;

using RepositoryInterfaces = TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

using DataContexts = TransferDesk.DAL.Manuscript.DataContext;
using TransferDesk.Contracts.Manuscript.ComplexTypes.Search;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using TransferDesk.Contracts.Manuscript.ComplexTypes.UserRole;

//todo: on refactoring all functions will be moved to repective repos.

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class ManuscriptDBRepositoryReadSide : IDisposable
    {

        public DataContexts.ManuscriptDBContext manuscriptDataContextRead;
        private int holdstatus_id;
        public ManuscriptDBRepositoryReadSide(string conString)
        {
            this.manuscriptDataContextRead = new DataContexts.ManuscriptDBContext(conString);
        }

        public IEnumerable<Entities.Manuscript> GetManuscripts()
        {
            return manuscriptDataContextRead.Manuscripts.ToList<Entities.Manuscript>();
        }

        public Entities.Manuscript GetManuscriptByID(int id)
        {

            return manuscriptDataContextRead.Manuscripts.Find(id);
        }

        public Entities.ManuscriptBookScreening GetManuscriptBookByID(int id)
        {
            return manuscriptDataContextRead.ManuscriptBookScreening.Find(id);
        }

        public List<Entities.BookMaster> GetManuscriptBookTitle()
        {
            var bookTitles = (from q in manuscriptDataContextRead.BookMaster
                where q.IsActive == true
                select q).ToList();
            return bookTitles;
        }

        public IEnumerable<pr_SearchMSDetails_Result> GetSearchResult(string SelectedValue, string SearchBy)
        {
            try
            {
                Nullable<int> value = Convert.ToInt32(SelectedValue);
                var selectedValueParameter = value.HasValue ?
              new SqlParameter("SelectedValue", value) :
              new SqlParameter("SelectedValue", typeof(global::System.Int32));

                var searchByParameter = SearchBy != null ?
                    new SqlParameter("SearchBy", SearchBy) :
                    new SqlParameter("SearchBy", typeof(global::System.String));
                IEnumerable<pr_SearchMSDetails_Result> empDetails = this.manuscriptDataContextRead.Database.SqlQuery
                                                                                  <pr_SearchMSDetails_Result>("exec pr_SearchMSDetails @SelectedValue, @SearchBy", selectedValueParameter, searchByParameter).ToList();
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

        public List<Entities.SearchByMaster> GetSearchList(string manuscriptType)
        {            
            var searchByList = (from q in manuscriptDataContextRead.SearchByMaster
                                where q.ManuscriptType.ToLower() == manuscriptType.ToLower()
                select q).ToList();
            return searchByList;
        }

        public List<Entities.Journal> GetJournalList()
        {
            var journalList = (from journals in manuscriptDataContextRead.Journals
                               where journals.IsActive == true
                               orderby journals.JournalTitle
                               select journals).ToList();
            return journalList;
        }

        public List<Entities.ArticleType> GetArticleTypeList(int journalID)
        {
            var journalArticles = from s in manuscriptDataContextRead.JournalArticleTypes.Where(x => x.JournalID == journalID)
                                  select s;
            var result = from ja in journalArticles join s in manuscriptDataContextRead.ArticleTypes on ja.ArticleTypeID equals s.ID select s;
            return result.ToList();
        }

        public int GetServiceID(int crestid)
        {
            int service_id = (from id in manuscriptDataContextRead.ManuscriptLoginDetails
                              where id.CrestId == crestid
                              select id.ServiceTypeStatusId).FirstOrDefault();
            return service_id;

        }

        public List<Entities.Section> GetSectionMasterList(int journalID)
        {
            var journalSections = from s in manuscriptDataContextRead.JournalSecions.Where(x => x.JournalID == journalID)
                                  select s;
            var result = from js in journalSections join s in manuscriptDataContextRead.Sections on js.SectionID equals s.ID select s;
            return result.ToList();
        }

        public List<Entities.Role> GetUserRoleList(int[] roleIds)
        {
            var roles = from q in manuscriptDataContextRead.Roles
                        where
                            roleIds.Contains(q.ID)
                        select q;
            return roles.ToList();
        }

        public List<Entities.ImageDropDownList> GetIthenticateResultList()
        {
            var imageDropDownList = from q in manuscriptDataContextRead.ImageDropDownList
                                    where q.ImageDropDownMenuID == 1
                                    select q;
            return imageDropDownList.ToList();
        }

        public List<Entities.ImageDropDownList> GetList(int ImageDropDownMenuID)
        {
            var imageDropDownList = from q in manuscriptDataContextRead.ImageDropDownList
                                    where q.ImageDropDownMenuID == ImageDropDownMenuID
                                    select q;
            return imageDropDownList.ToList();
        }

        public List<Entities.ArticleType> GetArticleList(int journalID)
        {
            var result = from r in manuscriptDataContextRead.ArticleTypes
                         join s in
                             (from q in manuscriptDataContextRead.JournalArticleTypes where q.JournalID == journalID && q.IsActive==true select q)
                             on r.ID equals s.ArticleTypeID
                         select r;
            return result.ToList();
        }

        public List<Entities.Section> GetSectionList(int journalID)
        {
            var result = from r in manuscriptDataContextRead.Sections
                         join s in
                             (from q in manuscriptDataContextRead.JournalSecions where q.JournalID == journalID && q.IsActive == true select q)
                             on r.ID equals s.SectionID
                         select r;
            return result.ToList();
        }

        public IEnumerable<Entities.Manuscript> GetManuscriptByMSID(string MSID)
        {
            var manuscripts = from m in manuscriptDataContextRead.Manuscripts
                              where m.MSID == MSID
                              select m;
            return manuscripts.ToList();
        }

        public List<Entities.OtherAuthor> GetOtherAuthors()
        {
            return manuscriptDataContextRead.OtherAuthors.ToList<Entities.OtherAuthor>();
        }

        public List<Entities.OtherAuthor> GetOtherAuthors(int manuscriptID)
        {
            var ManuscriptOtherAuthors = from q in manuscriptDataContextRead.OtherAuthors
                                         where q.ManuscriptID == manuscriptID
                                         select q;
            return ManuscriptOtherAuthors.ToList();
        }

        public Entities.OtherAuthor GetOtherAuthorByID(int id)
        {
            return manuscriptDataContextRead.OtherAuthors.Find(id);
        }

        public List<Entities.ManuscriptErrorCategory> GetManuscriptErrorCategoryList(int manuscriptID)
        {
            var ManuscriptErrorCategoryList = from q in manuscriptDataContextRead.ManuscriptErrorCategory
                                              where q.ManuscriptID == manuscriptID
                                              select q;
            return ManuscriptErrorCategoryList.ToList();
        }

        public Entities.ManuscriptErrorCategory GetManuscriptErrorCategory(int id)
        {
            return manuscriptDataContextRead.ManuscriptErrorCategory.Find(id);
        }

        public List<Entities.ErrorCategory> GetErrorCategoryList(string manuscriptType)
        {
            var errorCategoryList = (from q in manuscriptDataContextRead.ErrorCategories
                                     where q.ManuscriptType.ToLower() == manuscriptType
                select q).ToList();
            return errorCategoryList;
        }

       
        public Entities.ErrorCategory GetErrorCategory(int id)
        {
            return manuscriptDataContextRead.ErrorCategories.Find(id);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    manuscriptDataContextRead.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsMSIDAvailable(string msid, int id)
        {
            if (id == 0)
            {
                var result = from q in manuscriptDataContextRead.Manuscripts
                             where q.MSID == msid
                             select q;
                if (result.ToList().Count() == 0)
                    return true;
                else
                    return false;
            }
            else
            {
                var result = from q in manuscriptDataContextRead.Manuscripts
                             where q.MSID == msid
                             select q;
                var count = result.ToList().Count();
                if (result.ToList().Count() == 1)
                {
                    var pkCheck = from manuscript in result
                                  where manuscript.ID == id
                                  select manuscript;
                    if (pkCheck.ToList().Count() == 1)
                        return false;
                    else
                        return true;
                }
                else if (result.ToList().Count() > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string EmployeeName(string userID)
        {
            string userName = manuscriptDataContextRead.Database.SqlQuery<string>("SELECT EmpName from Employee Where UserName = '" + userID + "' and leftdate =''").FirstOrDefault<string>();
            return userName;
        }

        public int[] GetUserRoles(string userId)
        {
            var roleIds = from ur in manuscriptDataContextRead.UserRoles
                          join u in
                              (from user in manuscriptDataContextRead.Users select user)
                                     on ur.UserID equals u.EmpUserID
                          join r in
                              (from role in manuscriptDataContextRead.Roles select role)
                          on ur.RollID equals r.ID
                          where u.EmpUserID == userId && ur.IsActive == true
                          select ur.RollID;
            return roleIds.ToArray();

        }

        public object GetAssignedEditor(string searchText, string journalID)
        {

            int journalId = Convert.ToInt32(journalID);
            var assignedEditor = (from manuscript in manuscriptDataContextRead.Manuscripts
                                  where manuscript.JournalID == journalId && manuscript.AssignedEditor.Contains(searchText)
                                  select new
                                  {
                                      manuscript.AssignedEditor
                                  }).Distinct().ToList();
            return assignedEditor;
        }

        public int GetManuscriptID(string msid)
        {
               var result = (from q in manuscriptDataContextRead.Manuscripts
                             where q.MSID == msid
                             select new {
                             q.ID
                             }).ToList();

               return result.FirstOrDefault().ID;
        }

        public string GetArticleType(int articleTypeID)
        {
            return manuscriptDataContextRead.ArticleTypes.Find(articleTypeID).ArticleTypeName;
        }

        public string GetMetrixLegendTitle(int imageID)
        {
            if (imageID == 0)
                return "";
            else
                return manuscriptDataContextRead.ImageDropDownList.Find(imageID).DropDownText;
        }

        public string GetRole(int id)
        {
            return manuscriptDataContextRead.Roles.Find(id).RoleName;
        }

        public IEnumerable<pr_GetAssociateInfo_Result> GetAssociateName(string searchText)
        {
            try
            {
                var searchByParameter = searchText != null ?
                    new SqlParameter("SearchBy", searchText) :
                    new SqlParameter("SearchBy", typeof(global::System.String));
                IEnumerable<pr_GetAssociateInfo_Result> empDetails = this.manuscriptDataContextRead.Database.SqlQuery
                                                                                  <pr_GetAssociateInfo_Result>("exec pr_GetAssociateInfo @SearchBy", searchByParameter).ToList();
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

        public List<Entities.StatusMaster> GetServiceType()
        {
            var serviceType = from q in manuscriptDataContextRead.StatusMaster
                              where q.StatusCode == "Process" && q.IsActive == true
                              select q;
            return serviceType.ToList();
        }

        public List<Entities.StatusMaster> GetManuscriptStatus()
        {
            var manuscriptStatus = from q in manuscriptDataContextRead.StatusMaster
                                   where q.StatusCode == "ManuscriptStatus" && q.IsActive == true
                                   select q;
            return manuscriptStatus.ToList();
        }

        public List<Entities.JournalStatus> GetJournalStatusList()
        {
            var journalstatusList = (from journalstatus in manuscriptDataContextRead.JournalStatus
                                     where journalstatus.IsActive == true
                                     orderby journalstatus.Status
                                     select journalstatus).ToList();
            return journalstatusList;
        }

        public List<Entities.StatusMaster> GetTaskType()
        {
            var taskType = from q in manuscriptDataContextRead.StatusMaster
                           where q.StatusCode.ToLower() == "taskstatus" && q.IsActive == true
                           select q;
            return taskType.ToList();
        }

        public int GetStatusID(string status)
        {
            int status_id = (from id in manuscriptDataContextRead.StatusMaster
                             where id.Description.ToLower().Trim() == status.ToLower().Trim()
                             select id.ID).FirstOrDefault();
            return status_id;

        }

        public bool CheckJobStatusForHold(int crestid, int servicetype)
        {
            const string onHold = "On Hold";
            holdstatus_id = GetStatusID(onHold);
            try
            {
                var checkstatus = (from status in manuscriptDataContextRead.ManuscriptLoginDetails
                                   where status.CrestId == crestid && status.ServiceTypeStatusId == servicetype
                                   orderby status.Id descending
                                   select status.JobProcessStatusId).FirstOrDefault();
                if (checkstatus == null)
                {
                    return true;
                }
                else if (checkstatus == holdstatus_id)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception)
            {
                return true;
            }

        }
    }
}
