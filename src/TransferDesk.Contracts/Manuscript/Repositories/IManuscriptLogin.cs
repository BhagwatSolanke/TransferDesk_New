using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Repositories
{
    public interface IManuscriptLogin:IDisposable
    {
        void AddManuscriptLogin(Entities.ManuscriptLogin manuscriptLogin);
    }
}
