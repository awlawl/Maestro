﻿using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class NextController : ApiController
    {
        public void Next()
        {
            Player.Current.Next();
        }
        
    }
}
