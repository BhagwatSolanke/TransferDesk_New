using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.Entities;

namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class BookMasterVm
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please, Enter book title")]
        public string BookTitle { get; set; }
        public string GPUInformation { get; set; }
        public string FTPLink { get; set; }
        public bool IsActive { get; set; }
        public List<BookMaster> BookMasterData { get; set; }
    }
}
