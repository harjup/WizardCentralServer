using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Web.Compilation;
using System.Web.Http;
using Newtonsoft.Json;
using RestSharp;
using WizardCentralServer.Model.Dtos;
using WizardWebApi.Models.Objects;

namespace WizardWebApi.Controllers
{
    public class TweetsController : ApiController
    {
        //

        // GET: api/Tweets
        public IHttpActionResult Get()
        {
            var tokenRequest = new RestRequest("https://api.twitter.com/oauth2/token")
            {
                Method = Method.POST
            };
            tokenRequest.AddHeader("Authorization",
                ConfigurationManager.AppSettings["TwitterAuthKey"]);
            tokenRequest.AddParameter("grant_type", "client_credentials");

            var tokenResult = new RestClient().Execute(tokenRequest);
            var accessToken = JsonConvert.DeserializeObject<AccessToken>(tokenResult.Content);

            var request = new RestRequest(@"https://api.twitter.com/1.1/search/tweets.json?q=%23shrek%20OR%20%23dumbledore%20OR%20%23gandalf%20OR%20%23snape%20OR%20%23rincewind%20OR%20%23TerryPratchett&lang=en&result_type=recent&count=20")
            {
                Method = Method.GET
            };
            request.AddHeader("Authorization", "Bearer " + accessToken.token);

            var result = new RestClient().Execute(request);

            var tweets = JsonConvert.DeserializeObject<TwitterStatusResults>(result.Content);
            return Ok(tweets.Statuses);
        }
    }
}
