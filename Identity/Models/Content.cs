using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class Content
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ContentName { get; set; }
        [Required]
        public string ContentNumber { get; set; }
    }
}
