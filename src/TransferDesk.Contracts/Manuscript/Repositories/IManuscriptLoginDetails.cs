using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Repositories
{
    public interface IManuscriptLoginDetails : IDisposable
    {

        void AddManuscriptLoginDetails(Entities.ManuscriptLoginDetails manuscriptLoginDetails);
        void UpdateManuscriptLoginDetails(Entities.ManuscriptLoginDetails manuscriptLoginDetails);
        void SaveChanges();
    }
}
