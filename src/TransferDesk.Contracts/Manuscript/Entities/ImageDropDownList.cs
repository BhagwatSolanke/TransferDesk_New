using System.ComponentModel.DataAnnotations;
namespace TransferDesk.Contracts.Manuscript.Entities
{
    using System;
    using System.Collections.Generic;
    public class ImageDropDownList
    {
        [Key]
        public int ID { get; set; }
        public int ImageDropDownMenuID { get; set; }
        public string DropDownText { get; set; }
        public string ImageName { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> Modify_dttm { get; set; }
    }
}
