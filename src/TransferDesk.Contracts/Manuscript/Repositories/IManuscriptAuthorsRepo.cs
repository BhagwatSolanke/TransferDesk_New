
using System;
using System.Collections.Generic;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Contracts.Manuscript.Repositories
{
    public interface IManuscriptAuthorsRepo : IDisposable
    {
        IEnumerable<Entities.OtherAuthor> GetManuscriptAuthors();
      
    }
}
