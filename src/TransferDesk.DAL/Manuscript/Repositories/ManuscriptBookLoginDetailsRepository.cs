using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Repositories;
using DataContexts = TransferDesk.DAL.Manuscript.DataContext;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class ManuscriptBookLoginDetailsRepository
    {
        public DataContexts.ManuscriptDBContext context;
        private bool disposed = false;

        public ManuscriptBookLoginDetailsRepository(string conString)
        {
            this.context = new DataContexts.ManuscriptDBContext(conString);
        }
        public ManuscriptBookLoginDetailsRepository(DataContexts.ManuscriptDBContext context)
        {
            this.context = context;
        }
        public void AddManuscriptBookLoginDetails(Entities.ManuscriptBookLoginDetails manuscriptBookLoginDetails)
        {
            manuscriptBookLoginDetails.CreatedDate = System.DateTime.Now;
            context.ManuscriptBookLoginDetails.Add(manuscriptBookLoginDetails);
        }

        public void UpdateManuscriptBookLoginDetails(Entities.ManuscriptBookLoginDetails manuscriptBookLoginDetails)
        {
            manuscriptBookLoginDetails.ModifiedDate = System.DateTime.Now;
            Entities.ManuscriptBookLoginDetails existing = context.ManuscriptBookLoginDetails.Find(manuscriptBookLoginDetails.Id);
            ((IObjectContextAdapter)context).ObjectContext.Detach(existing);
            context.Entry(manuscriptBookLoginDetails).State = EntityState.Modified;
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

        public Entities.ManuscriptBookLoginDetails GetManuscriptBookLoginDetails(int ID, int ServiceTypeStatusId)
        {
            var manuscriptBookLoginDetails = (from q in context.ManuscriptBookLoginDetails
                                          where q.ManuscriptBookLoginId == ID && q.ServiceTypeStatusId == ServiceTypeStatusId && q.JobStatusId == 7
                                          select q).FirstOrDefault();
            return manuscriptBookLoginDetails;
        }

        public bool IsCrestIdGenerated(int manuscriptBookLoginId)
        {
            var crestId = (from q in context.ManuscriptBookLogin
                                       where q.ID == manuscriptBookLoginId && q.ManuscriptStatusID == 7
                                       select q.CrestID).FirstOrDefault();

            if (string.IsNullOrEmpty(crestId))
                return false;
            else
                return true;
        }

        public int ServiceTypeId(int manuscriptBookLoginId)
        {
            var serviceTypeId = (from q in context.ManuscriptBookLogin
                                 where q.ID == manuscriptBookLoginId && q.ManuscriptStatusID == 7 //&& q.ServiceTypeID == currentServiceTypeId
                                       select q.ServiceTypeID).FirstOrDefault();

            return Convert.ToInt32(serviceTypeId);
        }

        public int GetAssociateUserRoleId(int manuscriptBookLoginId)
        {
            var userRoleId = (from q in context.ManuscriptBookLoginDetails
                                 where q.ManuscriptBookLoginId == manuscriptBookLoginId && q.JobStatusId== 7 //&& q.ServiceTypeID == currentServiceTypeId
                                 select q.UserRoleId).FirstOrDefault();

            return Convert.ToInt32(userRoleId);
        }
        public Entities.UserRoles GetUserID(int userRoleID)
        {
            var userRole = context.UserRoles.Find(userRoleID);
            return userRole;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

            }
}
