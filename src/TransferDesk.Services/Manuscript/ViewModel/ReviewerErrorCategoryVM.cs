using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class ReviewerErrorCategoryVM
    {
        public int ID { get; set; }
        public int ErrorCategoryID { get; set; }
        public bool IsSelected { get; set; }
        public string ErrorCategoryName { get; set; }
    }
}
