using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.DTO;

namespace TransferDesk.Services.Manuscript.ViewModel
{
    public class ManuscriptErrorCategoryVM
    {
        public int ID { get; set; }
        public int ErrorCategoryID { get; set; }
        public bool IsSelected { get; set; }
        public string ErrorCategoryName { get; set; }
    }
}
