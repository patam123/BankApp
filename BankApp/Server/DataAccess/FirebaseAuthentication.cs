﻿using BankApp.Shared.Entities;
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
        public int MyProperty { get; set; }
        public async Task<string> CreateUser(User user)
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

        public async Task<UserResponse> LogIn(User user)
        {
            try
            {


                var filepath = @"C:\Users\patri\Desktop\bankapikey.txt";

                var apiKey = File.ReadAllText(filepath);

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