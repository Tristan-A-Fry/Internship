using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ecommapp.Models
{
    public class User
    {
        public int Id {get; set;}
        public string Username {get; set;}

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password {get; set;}

        public string Role {get; set;}
    }
}