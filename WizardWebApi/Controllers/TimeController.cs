using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using RestSharp;
using WizardWebApi.Models.Dtos;

namespace WizardWebApi.Controllers
{
    public class TimeController : ApiController
    {
        public IHttpActionResult Get()
        {
            var request = new RestRequest("http://www.timeapi.org/utc/now")
            {
                Method = Method.GET
            };
            request.AddHeader("Accept", "application/json");
            
            var result = new RestClient().Execute(request);
            var content = JsonConvert.DeserializeObject<TimeApiDate>(result.Content);
            return Ok(content);
        }
    }
}
