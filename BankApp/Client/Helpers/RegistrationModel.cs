using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Client.Helpers
{
    public class RegistrationModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Ditt lösenord måste vara 8-25 tecken långt")]
        public string Password { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 8)]
        public string RepeatPassword { get; set; }

    }
}
