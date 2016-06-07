using System;

using System.Data.Entity;
using System.Data.Entity.Core.Objects;

using Entities = TransferDesk.Contracts.Manuscript.Entities;
using CompleTypes = TransferDesk.Contracts.Manuscript.ComplexTypes;
using TransferDesk.Contracts.Manuscript.ComplexTypes.Search;

namespace TransferDesk.Contracts.Manuscript.DataContext
{
    public interface  IManuscriptDBContext : IDisposable
    {

        DbSet<Entities.Manuscript> Manuscripts { get; set; }
        DbSet<Entities.User> Users { get; set; }
        DbSet<Entities.Journal> Journals { get; set; }
        DbSet<Entities.JournalArticleTypes> JournalArticleTypes { get; set; }
        DbSet<Entities.JournalSections> JournalSecions { get; set; }
        DbSet<Entities.OtherAuthor> OtherAuthors { get; set; }
        DbSet<Entities.Section> Sections { get; set; }
        DbSet<Entities.Role> Roles { get; set; }
        DbSet<Entities.ErrorCategory> ErrorCategories { get; set; }
        DbSet<Entities.ArticleType> ArticleTypes { get; set; }

        int pr_GetMSDetails();
       
        int pr_GetUserMaster();
       
        int pr_MaxNumber(string tableName, string fieldName);
                   
       
    }
}



