using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AdminDashBoard;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AssociateDashBoard;
using TransferDesk.DAL.Manuscript.DataContext;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class AssociateDashBoardReposistory
    {

        private ManuscriptDBContext context;
        private bool disposed = false;


        public AssociateDashBoardReposistory(string conString)
        {
            this.context = new ManuscriptDBContext(conString);
        }

        public AssociateDashBoardReposistory(ManuscriptDBContext manuscriptDbContext)
        {
            this.context = manuscriptDbContext;  
        }

        public IEnumerable<pr_GetSpecificAssociateDetails_Result> pr_GetAllAssociatesAssignedJobs(string userid)
        {
            try
            {

                var associateuserid = userid != null ?
                   new SqlParameter("userid", userid) :
                   new SqlParameter("userid", typeof(global::System.String));

                IEnumerable<pr_GetSpecificAssociateDetails_Result> alljobsdetails = this.context.Database.SqlQuery
                                                                                  <pr_GetSpecificAssociateDetails_Result>("exec pr_GetAssociateAssignedJobs @userid", associateuserid).ToList();
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
