using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesAndHandlers.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Username { get; set; }
        [Required]
        [StringLength(128)]
        public string Password { get; set; }
    }
}
