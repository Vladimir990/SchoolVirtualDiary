using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using virtual_diary.Filters;

namespace virtual_diary.Models.DTO
{
    public class AdminDTO
    {
        public AdminDTO ()
        {

        }

      

        
        [Required]
        [StringLength(30,MinimumLength =2, ErrorMessage ="Name must be at least 2 characters long and maximum 30 caracters long")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength =2, ErrorMessage = "Last name must be at least 2 characters long and maximum 30 caracters long")]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15, MinimumLength =6, ErrorMessage ="Username must be at least 6 characters long and maximum 15 caracters long")]
        public string UserName { get; set; }

        [PasswordValid]
        [Compare("RepeatedPassword", ErrorMessage = "Passwords do not match!!!")]
        public string Password { get; set; }


        public string RepeatedPassword { get; set; }

    }
}