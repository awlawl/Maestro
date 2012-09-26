using System.Threading;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace RestAPI
{
    public class ApiHosting
    {
        private HttpSelfHostServer _server = null;
        
        public void Start()
        {
            var config = new HttpSelfHostConfiguration("http://localhost:20202");

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            _server = new HttpSelfHostServer(config);
            _server.OpenAsync().Wait();
            
        }

        public  void Stop()
        {
            _server.CloseAsync();
        }
        
    }
}
