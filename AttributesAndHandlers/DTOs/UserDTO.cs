using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesAndHandlers.DTOs
{
    public class UserDTO
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
