using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Hosting.Self;
using MusicData;

namespace Website
{
    public class SelfHostedWebsite
    {
        public NancyHost _nancyHost = null;

        public void Start() 
        {
            Log.Debug("Starting self hosted website");
            _nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:1234"));
            _nancyHost.Start();
            Log.Debug("Done starting self hosted website");

        }

        public void Stop()
        {
            _nancyHost.Stop();
        }

    }
}
