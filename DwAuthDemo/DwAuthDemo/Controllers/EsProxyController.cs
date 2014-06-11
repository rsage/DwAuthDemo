using System;
using System.Linq;
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
        [Route("EsProxy/{*op}")]
        // GET: EsProxy/
        public async Task<HttpResponseMessage> GetNodes(string op)
        {
            return await DoEsRequest(async c => await c.GetAsync(op));
        }

        [HttpPost]
        [Route("EsProxy/{*op}")]
        // POST: EsProxy/
        public async Task<HttpResponseMessage> PostQuery(string op, [FromBody] JObject query)
        {
            var queryToken = GetQueryToken(query);
            ChangeQuery(queryToken);
            return await DoEsRequest(async c => await c.PostAsync(op, query.ToString()));
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

        private static JToken GetQueryToken(JToken root)
        {
            if (!root.HasValues) return null;

            return root["query"] ?? root.Values().Select(GetQueryToken).FirstOrDefault(query => query != null);
        }

        private static void ChangeQuery(JToken query)
        {
            var array = (JArray)query["filtered"]["filter"]["bool"]["must"];

            var filter = new JObject
            {
                {
                    "fquery", new JObject
                    {
                        {"query", new JObject {{"query_string", new JObject {{"query", "project=public"}}}}}
                    }
                }
            };


            array.Add(filter);
        }
    }
}
