using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
namespace TransferDesk.Contracts.Manuscript.DTO
{
    public class ManuscriptLoginDTO
    {
        public Entities.ManuscriptLogin manuscriptLogin { get; set; }
        public List<Entities.ManuscriptLoginDetails> manuscriptLoginDetails { get; set; }

        public ManuscriptLoginDTO()
        {
            manuscriptLogin = new Entities.ManuscriptLogin();
            manuscriptLoginDetails = new List<Entities.ManuscriptLoginDetails>();
        }
        public string AssociateName { get; set; }
        public bool IsRevision{ get; set; }
        public bool IsCrestIDPresent { get; set; }
        public string userID { get; set; }
    }
}
