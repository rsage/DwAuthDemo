using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DwAuthDemo.Controllers
{
    [AllowAnonymous]
    public class EsAuthProxyController : ApiController
    {
        public string Get(string id)
        {
            return "Hello";
        }
    }
}
