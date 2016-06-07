using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferDesk.Contracts.Manuscript.Entities
{
    public class Location
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int? Locationtype { get; set; }
        public int? Isvisible { get; set; }
        public int? Parentid { get; set; }
    }
}
