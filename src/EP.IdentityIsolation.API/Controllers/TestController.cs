using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EP.IdentityIsolation.API.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("api/whoami")]
        public string GetUserName()
        {
            if (User.Identity.IsAuthenticated)
                return User.Identity.Name;

            return "anonymous";
        }
    }
}
