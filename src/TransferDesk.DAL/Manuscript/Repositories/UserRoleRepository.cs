
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;

using TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

using TransferDesk.DAL.Manuscript.DataContext;
using System.Data.Entity.Infrastructure;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.ComplexTypes.UserRole;


namespace TransferDesk.DAL.Manuscript
{
    public class UserRoleRepository : IDisposable
    {
        private ManuscriptDBContext context;

        public UserRoleRepository(ManuscriptDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<Entities.UserRoles> GetUserRoles()
        {
            return context.UserRoles.ToList<Entities.UserRoles>();
        }

        public Entities.UserRoles GetUserRoleByID(int id)
        {
            return context.UserRoles.Find(id);
        }

        public void AddUserRole(Entities.UserRoles userRoles)
        {
            context.UserRoles.Add(userRoles);
        }


        public void UpdateUserRole(Entities.UserRoles userRoles)
        {
            UserRoles existing = context.UserRoles.Find(userRoles.ID);
            ((IObjectContextAdapter)context).ObjectContext.Detach(existing);
            context.Entry(userRoles).State = EntityState.Modified;
        }

        public IEnumerable<Role> GetRoleByUserID(string userID)
        {
            var roles = from role in context.Roles
                        select role;
            return roles.ToList<Entities.Role>();
        }


        public IEnumerable<pr_GetUserRoleDetails_Result> GetUserRoleDetails()
        {
            try
            {
                IEnumerable<pr_GetUserRoleDetails_Result> userRoleDetails =
                    this.context.Database.SqlQuery<pr_GetUserRoleDetails_Result>("exec pr_GetUserRoleDetails").ToList();
                return userRoleDetails;

            }
            catch
            {
                return null;//todo:check and remove this trycatchhandler
            }
            finally
            {

            }

        }

        public void SaveUserRole()
        {
            context.SaveChanges();
        }


        private bool disposed = false;

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

        public bool IsUserRoleAvailable(int id, int rollId, string userId)
        {
            if (id == 0)
            {
                var count = (from q in context.UserRoles
                                where q.RollID == rollId && q.UserID == userId
                                select q).Count();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                var userRoles = from q in context.UserRoles
                                where q.RollID == rollId && q.UserID == userId
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

        public bool IsUserIDAvailable(string userID)
        {

            var count = (from q in context.Users
                         where q.EmpUserID == userID.Trim()
                         select q).Count();
            if (count > 0)
                return true; 
            else
                return false;
            
        }

        public bool IsAdmin(string userId)
        {
            var count=(from userAdmin in context.UserAdmin
                        where userAdmin.UserID==userId
                            select userAdmin).Count();
            if (count > 0)
                return true;
            else
                return false;
        }
    }
}
