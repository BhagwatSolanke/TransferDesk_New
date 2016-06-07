using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Contracts
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DTOs = TransferDesk.Contracts.Manuscript.DTO;
//Validations
using Validations = TransferDesk.BAL.Manuscript.Validations;
//UnitOfWork Manuscript Screening
using TransferDesk.DAL.Manuscript.UnitOfWork;
using TransferDesk.DAL.Manuscript.Repositories;

//todo:keep a seperate read side/done needs refactor later

//Developer Hint: All additional init functions are for performance optimizations when needed

namespace TransferDesk.BAL.Manuscript
{
    public class ManuscriptScreeningBL : IDisposable
    {
      
        public ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide { get; set; }

        public Validations.ManuscriptValidations _manuscriptValidations { get; set; } 

        public String _ConStringRead {get;set;}

         public String _ConStringWrite {get;set;}

         public ManuscriptScreeningBL()
         {
             //empty constructor
         }
 
        public ManuscriptScreeningBL(String ConStringRead, String ConStringWrite)
        {
            _ConStringRead = ConStringRead;
            _ConStringWrite = ConStringWrite;
            InitManuscriptScreeningBL();
        }

         public void InitManuscriptScreeningBL()
        {
            InitManuscriptDBRepositoryReadSide();

            InitManuscriptValidations();
        }

        public void InitManuscriptDBRepositoryReadSide()
        {
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(_ConStringWrite);
        }

        public void InitManuscriptValidations()
        {
            _manuscriptValidations = new Validations.ManuscriptValidations();

            if (_manuscriptDBRepositoryReadSide == null)
            {
                InitManuscriptDBRepositoryReadSide();
            }
        }



        public DTOs.ManuscriptScreeningDTO GetManuscriptScreeningDefaultDTO()
        {
            DTOs.ManuscriptScreeningDTO manuscriptDTO = new DTOs.ManuscriptScreeningDTO();
           
            manuscriptDTO.ErrorCategoriesList = _manuscriptDBRepositoryReadSide.GetErrorCategoryList("j");
            
            return manuscriptDTO;
          
        }

        public DTOs.ManuscriptBookScreeningDTO GetManuscriptBookScreeningDefaultDTO()
        {
            DTOs.ManuscriptBookScreeningDTO manuscriptDTO = new DTOs.ManuscriptBookScreeningDTO();

            manuscriptDTO.ErrorCategoriesList = _manuscriptDBRepositoryReadSide.GetErrorCategoryList("b");

            return manuscriptDTO;

        }

        public DTOs.ManuscriptScreeningDTO GetManuscriptScreeningDTO(int manuscriptID)
        {

            DTOs.ManuscriptScreeningDTO manuscriptDTO = GetManuscriptScreeningDefaultDTO();
            manuscriptDTO.Manuscript = _manuscriptDBRepositoryReadSide.GetManuscriptByID(manuscriptID);
            manuscriptDTO.OtherAuthors = _manuscriptDBRepositoryReadSide.GetOtherAuthors(manuscriptID);
            manuscriptDTO.manuscriptErrorCategoryList = _manuscriptDBRepositoryReadSide.GetManuscriptErrorCategoryList(manuscriptID);
         
            return manuscriptDTO;
        }

        public DTOs.ManuscriptBookScreeningDTO GetManuscriptBoookScreeningDTO(int bookid)
        {

            DTOs.ManuscriptBookScreeningDTO manuscriptDTO = GetManuscriptBookScreeningDefaultDTO();
            manuscriptDTO.ManuscriptBookScreening = _manuscriptDBRepositoryReadSide.GetManuscriptBookByID(bookid);
            manuscriptDTO.manuscriptErrorCategoryList = _manuscriptDBRepositoryReadSide.GetManuscriptErrorCategoryList(bookid);

            return manuscriptDTO;
        }

        public bool SaveManuscriptScreening(DTOs.ManuscriptScreeningDTO manuscriptScreeningDTO, IDictionary<string,string> dataErrors)
        {
            if (_manuscriptValidations == null)
            {
                InitManuscriptValidations();

                 _manuscriptValidations.Validate_MSID(manuscriptScreeningDTO.Manuscript, dataErrors);
                if (dataErrors.Count > 0)
                {
                    return false;
                }
                
            }

            //set system managed attributes

            //set starttime as system time for add of Manuscript
            if (manuscriptScreeningDTO.Manuscript.ID  == 0)
            {
                manuscriptScreeningDTO.Manuscript.StartDate = System.DateTime.Now;
            }

            //Set Quality user id if role is quality
            if (manuscriptScreeningDTO.Manuscript.RoleID == 2)//todo: set constants for roles
            {
                manuscriptScreeningDTO.Manuscript.QualityUserID = manuscriptScreeningDTO.CurrentUserID;
            }
            else
            {
                manuscriptScreeningDTO.Manuscript.UserID = manuscriptScreeningDTO.CurrentUserID;
            }

            //if a revision occurs, add the same manuscript as new manuscript with a parent 

             if (manuscriptScreeningDTO.AddedNewRevision == true)
             {
                 manuscriptScreeningDTO.Manuscript.ParentManuscriptID = manuscriptScreeningDTO.Manuscript.ID;
                 manuscriptScreeningDTO.Manuscript.ID = 0;

                 //also each related details will be added, for new revised manuscript
                 for(int counter = 0; counter < manuscriptScreeningDTO.OtherAuthors.Count; counter++)
                 {
                     manuscriptScreeningDTO.OtherAuthors[counter].ID = 0;
                 }
                 for (int counter = 0; counter < manuscriptScreeningDTO.manuscriptErrorCategoryList.Count; counter++)
                 {
                     manuscriptScreeningDTO.manuscriptErrorCategoryList[counter].ID = 0;
                 }
             }
            
            //set what to save for manuscript screening
             manuscriptScreeningDTO.HasToSaveManuscript = true;
             manuscriptScreeningDTO.HasToSaveOtherAuthors = true;
             manuscriptScreeningDTO.HasToSaveErrorCategoriesList = true;

            ManuscriptScreeningUnitOfWork _manuscriptScreeningUnitOfWork = null;
            try
            {
                _manuscriptScreeningUnitOfWork = new ManuscriptScreeningUnitOfWork(_ConStringWrite);

                _manuscriptScreeningUnitOfWork.manuscriptScreeningDTO = manuscriptScreeningDTO;
                _manuscriptScreeningUnitOfWork.SaveManuscriptScreening();
                _manuscriptScreeningUnitOfWork.SaveChanges();//todo:change this function to update ids and save as seperate commit
                return true;
            }
            //exception will be raised up in the call stack
            finally
            {
                if(_manuscriptScreeningUnitOfWork != null)
                {
                    _manuscriptScreeningUnitOfWork.Dispose();
                }
            }
        }

        //public IEnumerable<Entities.Manuscript> GetAllManuscript()
        //{
        //    return _manuscriptScreeningUnitOfWork.GetManuscriptList();  
        //}

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //_manuscriptScreeningUnitOfWork.Dispose(); 

                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
      

    }
}
