using System;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;

using System.Data.Entity.Core.Objects;

using Entities = TransferDesk.Contracts.Manuscript.Entities;
using CompleTypes = TransferDesk.Contracts.Manuscript.ComplexTypes;
using DataContexts = TransferDesk.Contracts.Manuscript.DataContext;
using TransferDesk.Contracts.Manuscript.ComplexTypes.Search;
using System.Data.SqlClient;
using TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptAdmin;

namespace TransferDesk.DAL.Manuscript.DataContext
{
    public class ManuscriptDBContext : DbContext, DataContexts.IManuscriptDBContext
    {
        
        public ManuscriptDBContext(string ConString)
            : base(ConString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //disable initializer this is mainly for production version if we dont want to lose existing data
            Database.SetInitializer<ManuscriptDBContext>(null);

            modelBuilder.Properties<DateTime>()
          .Configure(c => c.HasColumnType("datetime2"));

           
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Entities.Manuscript>();
            modelBuilder.Entity<Entities.User>();
            modelBuilder.Entity<Entities.Journal>();
            modelBuilder.Entity<Entities.JournalArticleTypes>();
            modelBuilder.Entity<Entities.JournalSections>();
            modelBuilder.Entity<Entities.Manuscript>();
            modelBuilder.Entity<Entities.OtherAuthor>();
            modelBuilder.Entity<Entities.Section>();
            modelBuilder.Entity<Entities.Role>();
            modelBuilder.Entity<Entities.ErrorCategory>();
            modelBuilder.Entity<Entities.ArticleType>();
            modelBuilder.Entity<Entities.SearchByMaster>();
            modelBuilder.Entity<Entities.ManuscriptErrorCategory>();
            modelBuilder.Entity<Entities.UserRoles>();
            modelBuilder.Entity<Entities.UserAdmin>();

            modelBuilder.Entity<Entities.MSReviewerErrorCategory>();
            modelBuilder.Entity<Entities.MSReviewersSuggestionInfo>();
            modelBuilder.Entity<Entities.MSReviewerLink>();
            modelBuilder.Entity<Entities.MSReviewersSuggestion>();
            modelBuilder.Entity<Entities.MSSAreaOfExpertise>();
            modelBuilder.Entity<Entities.MSSReviewerMail>();
            modelBuilder.Entity<Entities.StatusMaster>();
            modelBuilder.Entity<Entities.Location>();
            modelBuilder.Entity<Entities.ReferenceReviewerlink>();
            modelBuilder.Entity<Entities.ReviewerMailLink>();
            modelBuilder.Entity<Entities.ReviewerMaster>();
            modelBuilder.Entity<Entities.InstituteMaster>();
            modelBuilder.Entity<Entities.DepartmentMaster>();
            modelBuilder.Entity<Entities.EmailDetails>();
            modelBuilder.Entity<Entities.ReviewerErrorCategory>();
            modelBuilder.Entity<Entities.ManuscriptLogin>();
            modelBuilder.Entity<Entities.ManuscriptLoginDetails>();
            modelBuilder.Entity<Entities.JournalStatus>();
            modelBuilder.Entity<Entities.BookMaster>();
            modelBuilder.Entity<Entities.ManuscriptBookLogin>();
            modelBuilder.Entity<Entities.ManuscriptBookScreening>();

            modelBuilder.Entity<TransferDesk.Contracts.Manuscript.ComplexTypes.Search.pr_SearchMSDetails_Result>();
            modelBuilder.Entity<TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptAdmin.pr_GetJournalArticleDetails_Result>();
            modelBuilder.Entity<TransferDesk.Contracts.Manuscript.ComplexTypes.ManuscriptAdmin.pr_GetJournalSectionDetails_Result>();


        }

        public virtual DbSet<Entities.Manuscript> Manuscripts { get; set; }
        public virtual DbSet<Entities.User> Users { get; set; }
        public virtual DbSet<Entities.Journal> Journals { get; set; }
        public virtual DbSet<Entities.JournalArticleTypes> JournalArticleTypes { get; set; }
        public virtual DbSet<Entities.JournalSections> JournalSecions { get; set; }
        public virtual DbSet<Entities.OtherAuthor> OtherAuthors { get; set; }
        public virtual DbSet<Entities.Section> Sections { get; set; }
        public virtual DbSet<Entities.Role> Roles { get; set; }
        public virtual DbSet<Entities.ErrorCategory> ErrorCategories { get; set; }
        public virtual DbSet<Entities.ArticleType> ArticleTypes { get; set; }
        public virtual DbSet<Entities.SearchByMaster> SearchByMaster { get; set; }
        public virtual DbSet<Entities.ImageDropDownMenu> ImageDropDownMenu { get; set; }
        public virtual DbSet<Entities.ImageDropDownList> ImageDropDownList { get; set; }
        public virtual DbSet<Entities.ManuscriptErrorCategory> ManuscriptErrorCategory { get; set; }
        public virtual DbSet<Entities.UserRoles> UserRoles { get; set; }
        public virtual DbSet<Entities.UserAdmin> UserAdmin { get; set; }

        public virtual DbSet<Entities.StatusMaster> StatusMaster { get; set; }
        public virtual DbSet<Entities.MSReviewerErrorCategory> MSReviewerErrorCategory { get; set; }
        public virtual DbSet<Entities.MSReviewersSuggestionInfo> MSReviewersSuggestionInfo { get; set; }
        public virtual DbSet<Entities.MSReviewerLink> MSReviewerLink { get; set; }
        public virtual DbSet<Entities.MSReviewersSuggestion> MSReviewersSuggestion { get; set; }
        public virtual DbSet<Entities.MSSAreaOfExpertise> MSSAreaOfExpertise { get; set; }
        public virtual DbSet<Entities.MSSReviewerMail> MSSReviewerMail { get; set; }

        public virtual DbSet<Entities.Location> Location { get; set; }
        public virtual DbSet<Entities.ReferenceReviewerlink> ReferenceReviewerlink { get; set; }
        public virtual DbSet<Entities.ReviewerMailLink> ReviewerMailLink { get; set; }
        public virtual DbSet<Entities.ReviewerMaster> ReviewerMaster { get; set; }
        public virtual DbSet<Entities.InstituteMaster> InstituteMaster { get; set; }
        public virtual DbSet<Entities.DepartmentMaster> DepartmentMaster { get; set; }
        public virtual DbSet<Entities.EmailDetails> EmailDetails { get; set; }
        public virtual DbSet<Entities.ReviewerErrorCategory> ReviewerErrorCategory { get; set; }
        public virtual DbSet<Entities.ManuscriptLogin> ManuscriptLogin { get; set; }
        public virtual DbSet<Entities.ManuscriptLoginDetails> ManuscriptLoginDetails { get; set; }
        public virtual DbSet<Entities.ManuscriptBookLoginDetails> ManuscriptBookLoginDetails { get; set; }
        public virtual DbSet<Entities.JournalStatus> JournalStatus { get; set; }
        public virtual DbSet<Entities.BookMaster> BookMaster { get; set; }
        public virtual DbSet<Entities.ManuscriptBookLogin> ManuscriptBookLogin { get; set; }
        public virtual DbSet<Entities.ManuscriptBookScreening> ManuscriptBookScreening { get; set; }
        public virtual int pr_GetMSDetails()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pr_GetMSDetails");
        }

        public virtual int pr_GetUserMaster()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pr_GetUserMaster");
        }


        public virtual int pr_MaxNumber(string tableName, string fieldName)
        {
            var tableNameParameter = tableName != null ?
                new ObjectParameter("TableName", tableName) :
                new ObjectParameter("TableName", typeof(string));

            var fieldNameParameter = fieldName != null ?
                new ObjectParameter("FieldName", fieldName) :
                new ObjectParameter("FieldName", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pr_MaxNumber", tableNameParameter, fieldNameParameter);
        }

        public virtual ObjectResult<pr_GetJournalArticleDetails_Result> pr_GetJournalArticleDetails(Nullable<int> journalid)
        {
            var selectedValueParameter = journalid.HasValue ?
               new SqlParameter("journalid", journalid) :
               new SqlParameter("journalid", typeof(global::System.Int32));
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<pr_GetJournalArticleDetails_Result>("pr_GetJournalArticleDetails");
        }

        public virtual ObjectResult<pr_GetJournalSectionDetails_Result> pr_GetJournalSectionDetails(Nullable<int> journalid)
        {
            var selectedValueParameter = journalid.HasValue ?
               new SqlParameter("journalid", journalid) :
               new SqlParameter("journalid", typeof(global::System.Int32));
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<pr_GetJournalSectionDetails_Result>("pr_GetJournalSectionDetails");
        }



        public virtual ObjectResult<pr_SearchMSDetails_Result> pr_SearchMSDetails(Nullable<int> selectedValue, string searchBy)
        {
            var selectedValueParameter = selectedValue.HasValue ?
               new SqlParameter("SelectedValue", selectedValue) :
               new SqlParameter("SelectedValue", typeof(global::System.Int32));

            var searchByParameter = searchBy != null ?
                new SqlParameter("SearchBy", searchBy) :
                new SqlParameter("SearchBy", typeof(global::System.String));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<pr_SearchMSDetails_Result>("pr_SearchMSDetails");
        }
        
    }
}
