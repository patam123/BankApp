using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Shared.Entities
{
    public class RegisterApiModel
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
