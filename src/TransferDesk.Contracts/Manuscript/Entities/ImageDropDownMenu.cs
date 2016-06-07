using System.ComponentModel.DataAnnotations;
namespace TransferDesk.Contracts.Manuscript.Entities
{
    using System;
    using System.Collections.Generic;
    
    
    public partial class ImageDropDownMenu
    {
        [Key]
        public int ID { get; set; }
        public string DropDownMenu { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> Modify_dttm { get; set; }
    }
}
