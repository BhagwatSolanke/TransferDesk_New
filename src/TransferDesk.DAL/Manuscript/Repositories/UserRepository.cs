//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TransferDesk.DAL.Manuscript.Repositories
//{
//    class UserRepository
//    {
//    }
//}


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


namespace TransferDesk.DAL.Manuscript
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private ManuscriptDBContext context;

        public UserRepository(ManuscriptDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<Entities.User> GetUsers()
        {
            return context.Users.ToList<Entities.User>();
        }

        public Entities.User GetUserByID(int id)
        {
            return context.Users.Find(id);
        }

        public void AddUser(Entities.User user)
        {
            context.Users.Add(user);
        }


        public void UpdateUser(Entities.User user)
        {
            User existing = context.Users.Find(user.EmpID);
            ((IObjectContextAdapter)context).ObjectContext.Detach(existing);
            context.Entry(user).State = EntityState.Modified;
        }

        public void SaveUser()
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



        public bool IsUserAvailable(int? empId, string empUserId, int id)
        {
            //if (id == 0)
            //{
            //    var result = from q in context.Users
            //                 where q.EmpID == empId || q.EmpUserID == empUserId
            //                 select q;
            //    int count = result.ToList().Count();
            //    if (count > 0)
            //        return true;
            //    else
            //        return false;
            //}
            //else
            //{
            //    var result = from q in context.Users
            //                 where q.EmpID == empId || q.EmpUserID == empUserId
            //                 select q;
            //    if (result.ToList().Count() == 1)
            //    {
            //        var pkCheck = from user in result
            //                      where user.ID == id
            //                      select user;
            //        if (pkCheck.ToList().Count() == 1)
            //            return false;
            //        else
            //            return true;
            //    }
            //    else if (result.ToList().Count() > 1)
            //        return true;                
            //    else
            //        return false;
            //}

            return true;
        }
    }
}
