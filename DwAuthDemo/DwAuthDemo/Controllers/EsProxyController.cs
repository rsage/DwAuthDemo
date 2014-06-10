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
        public HttpResponseMessage GetNodes()
        {
            var conn = new ElasticConnection("localhost");
            var res = conn.Get("_nodes");
            var obj = JObject.Parse(res);
            var response = Request.CreateResponse(HttpStatusCode.OK, obj, "application/json");
            return response;
        }

        [HttpPost]
        [ActionName("_all")]
        // POST: EsProxy/_all
        public async Task<HttpResponseMessage> PostQuery(string op)
        {
            var query = await Request.Content.ReadAsStringAsync();
            var conn = new ElasticConnection("localhost");
            var res = conn.Post("_all/" + op, query);
            var obj = JObject.Parse(res);
            var response = Request.CreateResponse(HttpStatusCode.OK, obj, "application/json");
            return response;
        }
    }
}
