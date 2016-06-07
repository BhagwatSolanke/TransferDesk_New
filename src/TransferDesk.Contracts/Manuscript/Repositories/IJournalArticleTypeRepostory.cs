using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Repositories
{
  public interface IJournalArticleTypeRepostory : IDisposable
    {
        void AddArticleType(Entities.ArticleType journalarticletype);
        void UpdateArticleType(Entities.ArticleType journalarticletype);
        int articleID(string articleName);
        void AddJournalArticleType(Entities.JournalArticleTypes journalarticletype);
      
    }
}
