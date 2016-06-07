using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.DTO
{
    public class ManuscriptBookLoginDTO
    {
        public Entities.ManuscriptBookLogin manuscriptBookLogin { get; set; }
        public List<Entities.ManuscriptBookLoginDetails> manuscriptBookLoginDetails { get; set; }

        public ManuscriptBookLoginDTO()
        {
            manuscriptBookLogin = new Entities.ManuscriptBookLogin();
            manuscriptBookLoginDetails = new List<Entities.ManuscriptBookLoginDetails>();
        }
        public string AssociateName { get; set; }
        public string FTPLink { get; set; }
        public string GPUInformation { get; set; }
        public bool IsRevision{ get; set; }
        public bool IsCrestIDPresent { get; set; }
        public string userId { get; set; }
        public DateTime CreatedDate{ get; set; }
        public string CreatedBy { get; set; }
    }
}

