using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

using RepositoryInterfaces = TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

using DataContexts = TransferDesk.DAL.Manuscript.DataContext;
using TransferDesk.Contracts.Manuscript.ComplexTypes.Search;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using TransferDesk.Contracts.Manuscript.ComplexTypes.LocationInfo;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class ReviewerSuggestionDBRepositoryReadSide : IDisposable
    {
        public DataContexts.ManuscriptDBContext ReviewerSuggestionDataContextRead;

        public ReviewerSuggestionDBRepositoryReadSide(string conString)
        {
            this.ReviewerSuggestionDataContextRead = new DataContexts.ManuscriptDBContext(conString);
        }

        public dynamic GetTaskIDList()
        {
            var result = (from q in ReviewerSuggestionDataContextRead.StatusMaster
                          where q.StatusCode.ToLower() == "taskstatus"
                          select new
                          {
                              ID = q.ID,
                              Description = q.Description
                          }).ToList();
            return result;
        }

        public Entities.MSReviewersSuggestion GetManuscriptByID(int? reviewerId)
        {
            return ReviewerSuggestionDataContextRead.MSReviewersSuggestion.Find(reviewerId);
        }

        public List<Entities.MSReviewersSuggestion> GetManuscriptByMSID(int msid)
        {
            var result = (from q in ReviewerSuggestionDataContextRead.MSReviewersSuggestion
                          where q.MSID.Equals(msid)
                          select q).ToList();
            return result;
        }

        public Entities.MSReviewersSuggestionInfo GetMSReviewerInfoIDs(int msReviewersSuggestionId)
        {
            return ReviewerSuggestionDataContextRead.MSReviewersSuggestionInfo.Find(msReviewersSuggestionId);
        }

        public List<Entities.MSReviewersSuggestionInfo> GetMSReviewers(int msReviewersSuggestionId, string msid)
        {
            var msReviewersSuggestionInfo = new List<Entities.MSReviewersSuggestionInfo>();
            var result = (from q in ReviewerSuggestionDataContextRead.MSReviewersSuggestionInfo
                          where q.MSReviewersSuggestionID == msReviewersSuggestionId && q.IsAssociateFinalSubmit == true
                          orderby q.IsAssociateFinalSubmit ascending, q.AnalystSubmissionDate descending
                          select q).ToList();
            msReviewersSuggestionInfo.AddRange(result);
            return msReviewersSuggestionInfo;
        }

        public Entities.ReviewerMaster GetMSReviewerDetails(int reviewerMasterId)
        {
            return ReviewerSuggestionDataContextRead.ReviewerMaster.Find(reviewerMasterId);
        }

        public List<Entities.MSReviewerLink> GetMSReviewerLinks(int msReviewersSuggestionInfoId)
        {
            var msReviewerLinks = (from reviewerLinks in ReviewerSuggestionDataContextRead.MSReviewerLink
                                   where reviewerLinks.MSReviewersSuggestionInfoID == msReviewersSuggestionInfoId
                                   select reviewerLinks).ToList();
            return msReviewerLinks;
        }

        public List<Entities.MSSReviewerMail> GetMSSReviewerMails(int msReviewersSuggestionInfoId)
        {
            var result = (from q in ReviewerSuggestionDataContextRead.MSSReviewerMail
                          where q.MSReviewersSuggestionInfoID == msReviewersSuggestionInfoId
                          select q).ToList();
            return result;
        }

        public List<Entities.MSSAreaOfExpertise> GetMSSAreaOfExpertise(int msReviewersSuggestionInfoId)
        {
            var msAreaOfExpertises = (from aoe in ReviewerSuggestionDataContextRead.MSSAreaOfExpertise
                                      where aoe.MSReviewersSuggestionInfoID == msReviewersSuggestionInfoId
                                      select aoe).ToList();
            return msAreaOfExpertises;
        }

        public List<Entities.MSReviewerErrorCategory> GetErrorCategoryList(int? reviewerId)
        {
            var msErrorCategoryList = from q in ReviewerSuggestionDataContextRead.MSReviewerErrorCategory
                                      where q.MSReviewersSuggestionID == reviewerId
                                      select q;
            return msErrorCategoryList.ToList();
        }

        public List<Entities.ReviewerMaster> GetReviewerDetails(int reviewerMasterId)
        {
            var reviewerDetails = (from reviewer in ReviewerSuggestionDataContextRead.ReviewerMaster
                                   where reviewer.ID == reviewerMasterId
                                   select reviewer).ToList();
            return reviewerDetails;
        }

        public string GetInstituteDetails(int? institudeId)
        {
            if (institudeId != null)
            {
                var institudeName = (from institude in ReviewerSuggestionDataContextRead.InstituteMaster
                                     where institude.ID == institudeId
                                     select institude.Name).ToList();
                return institudeName[0].ToString();
            }
            else
                return string.Empty;
        }

        public List<pr_LocationInfo_Result> GetLocationDetails(int? deptid)
        {

            var cityID = deptid.HasValue ? new SqlParameter("cityID", deptid) : new SqlParameter("cityID", typeof(global::System.Int32));
            List<pr_LocationInfo_Result> locationDetails =
                  this.ReviewerSuggestionDataContextRead.Database.SqlQuery<pr_LocationInfo_Result>("exec pr_LocationInfo @cityID", cityID).ToList();
            return locationDetails;
        }

        //Added by Sambhaji Andhare.
        public List<pr_LocationInfo_Result> GetLocationDetailsForCleanData(int? reviewerId)
        {

            var reviewerMasterId = reviewerId.HasValue ? new SqlParameter("ReviewerMasterID", reviewerId) : new SqlParameter("ReviewerMasterID", typeof(global::System.Int32));
            List<pr_LocationInfo_Result> locationDetails =
                  this.ReviewerSuggestionDataContextRead.Database.SqlQuery<pr_LocationInfo_Result>("exec pr_LocationInfoForCleanData @ReviewerMasterId", reviewerMasterId).ToList();
            return locationDetails;
        }

        public string GetDepartmentDetails(int? deptid)
        {
            if (deptid != null)
            {
                var deptName = (from dept in ReviewerSuggestionDataContextRead.DepartmentMaster
                                where dept.ID == deptid
                                select dept.Name).ToList();
                return deptName[0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public IEnumerable<pr_ReviewerDetails_Result> GetReviewerSearchResult(string selectedValue, string searchBy)
        {
            try
            {
                int? value = Convert.ToInt32(selectedValue);
                var selectedValueParameter = value.HasValue ?
              new SqlParameter("SelectedValue", value) :
              new SqlParameter("SelectedValue", typeof(global::System.Int32));

                var searchByParameter = searchBy != null ?
                    new SqlParameter("SearchBy", searchBy) :
                    new SqlParameter("SearchBy", typeof(global::System.String));
                IEnumerable<pr_ReviewerDetails_Result> empDetails = this.ReviewerSuggestionDataContextRead.Database.SqlQuery
                                                                                  <pr_ReviewerDetails_Result>("exec pr_ReviewerDetails @SelectedValue, @SearchBy", selectedValueParameter, searchByParameter).ToList();
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

        public bool IsMSIDAvailable(string msid, int manuscriptId)
        {
            if (manuscriptId == 0)
            {
                var msReviewersSuggestion = from manuscriptInfo in ReviewerSuggestionDataContextRead.MSReviewersSuggestion
                                            where manuscriptInfo.MSID == msid
                                            select manuscriptInfo;
                return msReviewersSuggestion.ToList().Count() == 0;
            }
            else
            {
                var msReviewersSuggestion = from manuscriptInfo in ReviewerSuggestionDataContextRead.MSReviewersSuggestion
                                            where manuscriptInfo.MSID == msid
                                            select manuscriptInfo;
                if (msReviewersSuggestion.ToList().Count() == 1)
                {
                    var pkCheck = from manuscript in msReviewersSuggestion
                                  where manuscript.ID == manuscriptId
                                  select manuscript;
                    return pkCheck.ToList().Count() != 1;
                }
                else if (msReviewersSuggestion.ToList().Count() > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public object GetMSReviewersSuggestionIDs(string msid)
        {
            var msReviewersSuggestionIDs = (from msreviewer in ReviewerSuggestionDataContextRead.MSReviewersSuggestion
                                            where (msreviewer.MSID).Contains(msid)
                                            select msreviewer.ID).ToList();
            return 0;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    ReviewerSuggestionDataContextRead.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public object GetJounralName(int? journalId)
        {
            var journalName = (from journalinfo in ReviewerSuggestionDataContextRead.Journals
                               where journalinfo.ID == journalId
                               select new
                               {
                                   journalinfo.JournalTitle
                               }).FirstOrDefault().JournalTitle;
            return journalName;
        }

        public Entities.Employee GetAssociateInfo(string userId)
        {
            var userid = userId != null ? new SqlParameter("userId", userId) : new SqlParameter("userId", typeof(global::System.String));
            List<Entities.Employee> empInfo =
                  this.ReviewerSuggestionDataContextRead.Database.SqlQuery<Entities.Employee>("exec pr_GetEmpInfo @userId", userid).ToList();
            return empInfo.FirstOrDefault();
        }

        public List<Entities.ReviewerErrorCategory> GetReviewerErrorCategoryList()
        {
            return ReviewerSuggestionDataContextRead.ReviewerErrorCategory.ToList<Entities.ReviewerErrorCategory>();
        }
    }
}
