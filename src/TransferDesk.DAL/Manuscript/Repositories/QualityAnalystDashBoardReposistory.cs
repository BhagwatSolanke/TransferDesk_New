using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.QualityAnalystDashBoard;
using TransferDesk.DAL.Manuscript.DataContext;

namespace TransferDesk.DAL.Manuscript.Repositories
{
   public class QualityAnalystDashBoardReposistory
    {

        private ManuscriptDBContext context;
        private bool disposed = false;


        public QualityAnalystDashBoardReposistory(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public IEnumerable<pr_GetSpecificQualityAnalystJobs_Result> pr_GetAllQualityAnalystAssignedJobs(string userid)
        {
            try
            {

                var qualityassociateid = userid != null ?
                   new SqlParameter("userid", userid) :
                   new SqlParameter("userid", typeof(global::System.String));

                IEnumerable<pr_GetSpecificQualityAnalystJobs_Result> alljobsdetails = this.context.Database.SqlQuery
                                                                                  <pr_GetSpecificQualityAnalystJobs_Result>("exec pr_GetQualityAnalystAssignedJobs @userid", qualityassociateid).ToList();
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
