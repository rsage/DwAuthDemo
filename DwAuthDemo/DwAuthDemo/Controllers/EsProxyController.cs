using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using PlainElastic.Net;

namespace DwAuthDemo.Controllers
{
    [AllowAnonymous]
    public class EsProxyController : ApiController
    {
        [HttpGet]
        [ActionName("_nodes")]
        // GET: EsProxy/_nodes
        public async Task<HttpResponseMessage> GetNodes()
        {
            return await DoEsRequest(async c => await c.GetAsync("_nodes"));
        }

        [HttpPost]
        [ActionName("_all")]
        // POST: EsProxy/_all
        public async Task<HttpResponseMessage> PostQuery(string op)
        {
            return await DoEsRequest(async (c) => await c.PostAsync("_all/" + op, await Request.Content.ReadAsStringAsync()));
        }

        private async Task<HttpResponseMessage> DoEsRequest(Func<ElasticConnection, Task<string>> esRequest)
        {
            var conn = GetEsConnection();

            var res = await esRequest(conn);

            return CreateJsonResponse(res);
        }

        private static ElasticConnection GetEsConnection()
        {
            return new ElasticConnection("localhost");
        }

        private HttpResponseMessage CreateJsonResponse(string res)
        {
            var obj = JObject.Parse(res);
            var response = Request.CreateResponse(HttpStatusCode.OK, obj, "application/json");
            return response;
        }
    }
}
