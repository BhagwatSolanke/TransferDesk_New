using System;
using System.Collections.Generic;
using Entities = TransferDesk.Contracts.Manuscript.Entities; 

namespace TransferDesk.Contracts.Manuscript.Repositories
{
    public interface IUserRepository:IDisposable
    {
        IEnumerable<Entities.User> GetUsers();
        Entities.User GetUserByID(int ID);
        void AddUser(Entities.User user);
        void UpdateUser(Entities.User user);
        void SaveUser();
    }
}
