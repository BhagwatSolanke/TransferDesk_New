using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AdminDashBoard;
using TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptAdmin;
using TransferDesk.DAL.Manuscript.DataContext;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class AdminDashBoardReposistory
    {

        private ManuscriptDBContext context;
        private bool disposed = false;

        public AdminDashBoardReposistory(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }
        public AdminDashBoardReposistory(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;
        }

        public IEnumerable<pr_AdminDashBoardGetAllOpenJobs_Result> pr_GetAllJobsDetails()
        {
            try
            {

                IEnumerable<pr_AdminDashBoardGetAllOpenJobs_Result> alljobsdetails = this.context.Database.SqlQuery
                                                                                  <pr_AdminDashBoardGetAllOpenJobs_Result>("exec pr_AdminDashBoardGetAllOpenJobs").ToList();
                return alljobsdetails;
            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }
        }
        public List<pr_AdminDashBoardGetAllOpenJobsForExport_Result> GetAdminDashBoardExportToExcel(string FromDate1, string ToDate1)
        {
            try
            {
                var FromDate = Convert.ToDateTime(FromDate1);
                var ToDate = Convert.ToDateTime(ToDate1);

                var FromDateParameter = FromDate != null ?
              new SqlParameter("FromDate", FromDate) :
              new SqlParameter("FromDate", typeof(global::System.DateTime));

                var ToDateParameter = ToDate != null ?
                    new SqlParameter("ToDate", ToDate) :
                    new SqlParameter("ToDate", typeof(global::System.DateTime));

                List<pr_AdminDashBoardGetAllOpenJobs_Result> AdminDashBoardExportToExcel =
             this.context.Database.SqlQuery<pr_AdminDashBoardGetAllOpenJobs_Result>("pr_AdminDashBoardExportToExcel @FromDate, @ToDate", FromDateParameter, ToDateParameter).ToList();

                var AdminDashBoardExportToExcelData = (from q in AdminDashBoardExportToExcel
                                                       select new pr_AdminDashBoardGetAllOpenJobsForExport_Result()
                                                       {
                                                           SrNo = q.SrNo,
                                                           CrestId = q.CrestId,
                                                           JobType = q.JobType,
                                                           ServiceType = q.ServiceType,
                                                           MSID = q.MSID,
                                                           JournalBookName = q.JournalBookName,
                                                           PageCount = q.PageCount,
                                                           Name = q.Name,
                                                           Role = q.Role,
                                                           Status = q.Status,
                                                           Task = q.Task,
                                                           Revision = q.Revision,
                                                           GroupNo = q.GroupNo,
                                                           CreatedDate = q.CreatedDate,
                                                           ReceivedDate = q.ReceivedDate.HasValue ? q.ReceivedDate.Value.ToString("dd/MM/yyyy") : String.Empty,
                                                           FetchedDate = q.FetchedDate,
                                                           Age = q.Age,
                                                           HandlingTime = q.HandlingTime

                                                       }).ToList<pr_AdminDashBoardGetAllOpenJobsForExport_Result>();
                return AdminDashBoardExportToExcelData;
            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }

        }

        public List<pr_GetLastAssociateName_Result> GetLastNameOfAssociate_ForHoldJob(int crestid, string servicetype)
        {

            int serviceid = context.Database.SqlQuery<Int32>("SELECT ID from StatusMaster Where Description = '" + servicetype + "'").FirstOrDefault<Int32>();
            try
            {
                var crestidbyParameter = crestid != null ?
                    new SqlParameter("CrestID", crestid) :
                    new SqlParameter("CrestID", typeof(global::System.Int32));

                var servicetypebyparameter = serviceid != null ?
                    new SqlParameter("ServiceType", serviceid) :
                    new SqlParameter("ServiceType", typeof(global::System.Int32));

                List<pr_GetLastAssociateName_Result> associatename = this.context.Database.SqlQuery<pr_GetLastAssociateName_Result>("exec pr_GetLastAssociateName @CrestID,@ServiceType", crestidbyParameter, servicetypebyparameter).ToList();
                return associatename;

            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }

            return null;
        }


        public IEnumerable<pr_GetAssociate> GetAssociateResult(string searchAssociate, string RoleName)
        {
            try
            {
                var associateByParameter = RoleName != null ?
                    new SqlParameter("RoleName", RoleName) :
                    new SqlParameter("RoleName", typeof(global::System.String));

                var searchAssociateParameter = searchAssociate != null ?
                    new SqlParameter("searchAssociate", searchAssociate) :
                    new SqlParameter("searchAssociate", typeof(global::System.String));
                IEnumerable<pr_GetAssociate> empDetails = this.context.Database.SqlQuery<pr_GetAssociate>("exec pr_GetAssociate @searchAssociate,@RoleName", searchAssociateParameter, associateByParameter).ToList();
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
