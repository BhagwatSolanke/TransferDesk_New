using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Repositories
{
    public interface IManuscriptBookLoginDetails : IDisposable
    {
        void AddManuscriptBookLoginDetails(Entities.ManuscriptBookLoginDetails manuscriptLoginDetails);
        void UpdateManuscriptBookLoginDetails(Entities.ManuscriptBookLoginDetails manuscriptLoginDetails);
        void SaveChanges();
    }
}
