using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
namespace TransferDesk.Contracts.Manuscript.DTO
{
   public class ManuscriptBookScreeningDTO
    {
       public Entities.ManuscriptBookScreening ManuscriptBookScreening;
        public bool AddedNewRevision;
        public bool HasToSaveManuscript;
        public bool HasToSaveOtherAuthors;
        public bool HasToSaveErrorCategoriesList;
       public int RollID { get; set; }
       public Entities.ManuscriptBookLogin ManuscriptBookLogin { get; set; }
       public List<Entities.ManuscriptErrorCategory> manuscriptErrorCategoryList { get; set; }
        public List<Entities.ErrorCategory> ErrorCategoriesList { get; set; }
        public ManuscriptBookScreeningDTO()
        {
            ManuscriptBookScreening = new TransferDesk.Contracts.Manuscript.Entities.ManuscriptBookScreening();
             manuscriptErrorCategoryList = new List<Entities.ManuscriptErrorCategory>();
             ErrorCategoriesList = new List<Entities.ErrorCategory>();
            ManuscriptBookLogin = new Entities.ManuscriptBookLogin();
        }
    }
}
