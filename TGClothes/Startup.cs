using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(TGClothes.Startup))]
namespace TGClothes
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Map SignalR hubs
            app.MapSignalR();
        }
    }
}