using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.DTO
{
    public class AdminDashBoardDTO
    {
        public List<Entities.ManuscriptLoginDetails> manuscriptLoginDetails { get; set; }
        public int CrestId { get; set; }
        public string ServiceType { get; set; }
        public string Role { get; set; }
        public string JobProcessingStatus { get; set; }
        public string AssociateName { get; set; }
        public AdminDashBoardDTO()
        {
            manuscriptLoginDetails = new List<Entities.ManuscriptLoginDetails>();
        }
        
    }
}
