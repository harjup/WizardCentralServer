using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WizardWebApi.Models.Objects
{
    public class AccessToken
    {

        [JsonProperty("access_token")]
        public string token;
        [JsonProperty("token_type")]
        public string type;
    }
}