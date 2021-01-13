using BankApp.Shared.Entities;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Server.DataAccess
{
    public class FirebaseAuthentication
    {
        public async Task<string> CreateUser(RegisterApiModel registrationModel)
        {
            UserRecordArgs args = new UserRecordArgs()
            {
                Email = registrationModel.Email,
                Password = registrationModel.Password,
                EmailVerified = false,
                DisplayName = registrationModel.FirstName + " " + registrationModel.LastName,

            };

            UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
            Console.WriteLine(userRecord.Uid);
            return userRecord.Uid;
        }
    }
}
