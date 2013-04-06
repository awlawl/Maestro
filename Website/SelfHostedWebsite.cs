using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Hosting.Self;

namespace Website
{
    public class SelfHostedWebsite
    {
        public NancyHost _nancyHost = null;

        public void Start() 
        {
            _nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:1234"));
            _nancyHost.Start();

        }

        public void Stop()
        {
            _nancyHost.Stop();
        }

    }
}
