using System.Collections.Generic;
using Entities = TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Contracts.Manuscript.DTO
{
    public class ManuscriptScreeningDTO
    {

        public Entities.Manuscript Manuscript;
        public string CurrentUserID;
        public bool AddedNewRevision;
        public bool HasToSaveManuscript;
        public bool HasToSaveOtherAuthors;
        public bool HasToSaveErrorCategoriesList;
        public bool HasAuthorDetails { get; set; }
        public List<Entities.OtherAuthor> OtherAuthors { get; set; }
        public List<Entities.ManuscriptErrorCategory> manuscriptErrorCategoryList { get; set; }
        public List<Entities.ErrorCategory> ErrorCategoriesList { get; set; }
        public ManuscriptScreeningDTO()
        {
             Manuscript = new TransferDesk.Contracts.Manuscript.Entities.Manuscript();//todo:rename namespace, can name it as Manuscript Types
             OtherAuthors = new List<Entities.OtherAuthor>();
             manuscriptErrorCategoryList = new List<Entities.ManuscriptErrorCategory>();

             ErrorCategoriesList = new List<Entities.ErrorCategory>();
            
        }
    }
}