using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BankApp.Shared.Entities
{
    [JsonObject]
    public class UserResponse
    {
        [JsonProperty(PropertyName = "localId")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "idToken")]
        public string IdToken { get; set; }
        public string ErrorMessage { get; set; }
        public string DisplayName { get; set; }
    }
}
