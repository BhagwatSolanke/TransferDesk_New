using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entities = TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.DAL.Manuscript.Repositories;

//Developer Hint: All additional init functions are for performance optimizations when needed

namespace TransferDesk.BAL.Manuscript.Validations
{
    public class ManuscriptValidations
    {
        public ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide { get; set; }

        public ManuscriptValidations()
        {
        }

        private void InitManuscriptDBRepositoryReadSide(string conStringManuscriptDB)
        {
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conStringManuscriptDB);
        }

        public void Validate_MSID(Entities.Manuscript manuscript, IDictionary<String,String> dataErrors)
        {
            IEnumerable<Entities.Manuscript> Manuscripts = _manuscriptDBRepositoryReadSide.GetManuscriptByMSID(manuscript.MSID);
            if ((Manuscripts != null)
                && (Manuscripts.Count() == 1) && Manuscripts.FirstOrDefault().ID == manuscript.ID)
            {
               //found the same manuscript only with this MSID
                return;
            }
            else
            {
                dataErrors.Add("MSID", "This Manuscript number already exist");
                return;
            }
        }
    }
}
;