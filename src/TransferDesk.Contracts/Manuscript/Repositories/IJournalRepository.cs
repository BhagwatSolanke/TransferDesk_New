using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Repositories
{
     public interface IJournalRepository:IDisposable

    {

     //   IEnumerable<Entities.Journal> GetJournals();
        //Entities.Journal GetJournalByID(int ID);
        void AddJournal(Entities.Journal user);
        void UpdateJournal(Entities.Journal user);
   //     void SaveJournal();
    }
}
