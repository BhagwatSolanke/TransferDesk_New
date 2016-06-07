using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
  public  class JournalStatus
    {
      [Key]
      public int ID {get; set;}
      public string Status {get; set;}
      public bool IsActive { get; set; }
    }
}
