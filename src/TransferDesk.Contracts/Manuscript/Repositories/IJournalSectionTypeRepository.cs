using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Repositories
{
    public interface IJournalSectionTypeRepository:IDisposable
    {
        void AddSection(Entities.Section sectiontype);
        void UpdateSection(Entities.Section sectiontype);
        int sectionID(string sectionName);
        void AddJournalSectionType(Entities.JournalSections journalsectiontype);

    }
}
