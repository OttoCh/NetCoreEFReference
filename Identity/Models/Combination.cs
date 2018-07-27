using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class Combination
    {
        [Key]
        public int CombinationId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Content> CombinationNumber { get; set; } 
    }

}
