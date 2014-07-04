using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PublicAccessApi.Controllers
{
    using System.IO;

    //Serves up crossdomain.xml to Unity because they're a dumb idiot
    public class CrossDomainController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage
                               {
                                   Content =
                                       new StreamContent(
                                       File.Open(
                                           AppDomain.CurrentDomain.BaseDirectory + "crossdomain.xml",
                                           FileMode.Open))
                               };
            return response;
        }
    }
}
