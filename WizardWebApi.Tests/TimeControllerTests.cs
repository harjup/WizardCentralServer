using System;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WizardWebApi.Controllers;
using WizardWebApi.Models.Dtos;

namespace WizardWebRole.Tests
{
    [TestClass]
    public class TimeControllerTests
    {
        [TestMethod]
        public void TestMethod1()
        {//dateString: "7/3/2014 9:53:03 PM"
            var controller = new TimeController();
            var result = controller.Get() as OkNegotiatedContentResult<TimeApiDate>;
            Console.WriteLine(result.Content);
            Assert.IsNotNull(result.Content);
        }


        [TestMethod]
        public void DateStringDeserializesAsExpected()
        {
            var content = JsonConvert.DeserializeObject<TimeApiDate>("{dateString: \"7/3/2014 9:53:03 PM\"}");
            Assert.IsNotNull(content);
        }
    }
}
