using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using RestSharp;
using WizardWebApi.Models;

namespace WizardWebApi.Controllers
{
    public class CommentsController : ApiController
    {

        private static SuperWizardDBEntities _superWizardContext;

        [HttpGet]
        [Route("api/comments/{time?}")]
        public IEnumerable<UserComment> Get(int time = -1)
        {
            var messages = new List<UserComment>();

            using (_superWizardContext = new SuperWizardDBEntities())
            {
                IQueryable<UserComment> commentQuery = null;
                //If time is not specified, just get most recent
                if (time == -1)
                {
                    commentQuery = (from c in _superWizardContext.UserComments
                                        orderby c.DateTime descending
                                        select c).Take(15);
                }
                else
                {
                    //TODO: Make sessionTime not nullable

                    //Here's a query!!
                    //If it works the way it's supposed to it'll order comments by when they were posted,
                    //Then get the ones closest to the current sessionTime
                    commentQuery = _superWizardContext.UserComments
                        .OrderByDescending(c => c.DateTime)
                        .Select(c => new {c, distance = Math.Abs(c.SessionTime.Value - time)})
                        .OrderBy(p => p.distance)
                        .Select(p => p.c)
                        .Take(15);
                }
                

                try
                {
                    foreach (var comment in commentQuery)
                    {
                        messages.Add(comment);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }



            return messages;
        }

        // POST: api/Values
        public IHttpActionResult Post(UserComment comment)
        {
            using (_superWizardContext = new SuperWizardDBEntities())
            {
                comment.DateTime = DateTime.Now;

                try
                {
                    _superWizardContext.UserComments.Add(comment);
                    _superWizardContext.SaveChanges();
                    return Ok();
                }
                catch (Exception exception)
                {
                    return InternalServerError(exception);
                }

            }
        }
    }
}
