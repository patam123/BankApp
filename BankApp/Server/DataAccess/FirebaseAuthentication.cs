using BankApp.Shared.Entities;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;

namespace BankApp.Server.DataAccess
{
    public class FirebaseAuthentication
    {
        public async Task<string> CreateUser(User user)
        {
            try
            {
                UserRecordArgs args = new UserRecordArgs()
                {
                    Email = user.Email,
                    Password = user.Password,
                    EmailVerified = false,
                    DisplayName = user.FirstName + " " + user.LastName,

                };

                UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
                Console.WriteLine(userRecord.Uid);
                return userRecord.Uid;
            }
            catch (FirebaseAuthException)
            {

                return "A user with this email already exists.";
            }

        }

        public async Task<string> UpdateUser(User user)
        {
            try
            {
                UserRecordArgs userRecordArgs = new UserRecordArgs();
                if (!string.IsNullOrEmpty(user.Password))
                {
                    userRecordArgs.Uid = user.Id;
                    userRecordArgs.Email = user.Email;
                    userRecordArgs.Password = user.Password;
                    userRecordArgs.DisplayName = user.FirstName + " " + user.LastName;
                }
                else
                {
                    userRecordArgs.Uid = user.Id;
                    userRecordArgs.Email = user.Email;
                    userRecordArgs.DisplayName = user.FirstName + " " + user.LastName;
                };
                var userRecord = await FirebaseAuth.DefaultInstance.UpdateUserAsync(userRecordArgs);
                if (!string.IsNullOrEmpty(userRecord.Uid))
                {
                    return "Användare uppdaterad";
                }
                else
                {
                    return "Något gick fel. Uppdatering misslyckades";
                }
            }
            catch (FirebaseAuthException)
            {

                return "Något gick fel. Uppdatering misslyckades";
            }
        }

        public async Task<string> DeleteUser(string id)
        {
            try
            {
                await FirebaseAuth.DefaultInstance.DeleteUserAsync(id);
                return "Användaren raderades.";
            }
            catch (FirebaseAuthException)
            {

                return "Något gick fel. Användaren kunde inte raderas.";
            }
        }

        public async Task<UserResponse> LogIn(User user)
        {
            try
            {
                var filepath = @"firebase.json";

                var apiKeyFile = File.ReadAllText(filepath);

                var apiKeyDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiKeyFile);

                string apiKey; 
                apiKeyDictionary.TryGetValue("KEY", out apiKey);

                var http = new HttpClient();

                var msg = new HttpRequestMessage(HttpMethod.Post, $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}");
                var contentBody = $"{{\"email\":\"{user.Email}\",\"password\":\"{user.Password}\",\"returnSecureToken\":true}}";
                var content = new StringContent(contentBody);
                msg.Content = content;
                var response = await http.SendAsync(msg);
                var responseMsg = await response.Content.ReadAsStringAsync();

                var userAuth = JsonConvert.DeserializeObject<UserResponse>(responseMsg);

                return userAuth;
            }

            catch (Exception ex)
            {
                var userAuth = new UserResponse();
                userAuth.ErrorMessage = ex.Message;
                return userAuth;
            }
        }
    }
}
