using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

namespace WebApi.Controllers
{
    public class MessagingController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var currentContext = HttpContext.Current;
            if (currentContext.IsWebSocketRequest ||
                currentContext.IsWebSocketRequestUpgrading)
            {
                currentContext.AcceptWebSocketRequest(ProcessWebsocketSession);
            }

            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        private async Task ProcessWebsocketSession(AspNetWebSocketContext context)
        {
            var handler = new MyWebSocketHandler();
             await handler.ProcessWebSocketRequestAsync(context);
            //return processTask;
        }

        //public HttpResponseMessage Post(string message)
        //{
        //    var currentContext = HttpContext.Current;
        //    if (currentContext.IsWebSocketRequest ||
        //        currentContext.IsWebSocketRequestUpgrading)
        //    {
        //        currentContext.AcceptWebSocketRequest(ProcessWebsocketSession);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        //}

        public HttpResponseMessage Post(byte[] message)
        {
            var currentContext = HttpContext.Current;
            if (currentContext.IsWebSocketRequest ||
                currentContext.IsWebSocketRequestUpgrading)
            {
                currentContext.AcceptWebSocketRequest(ProcessWebsocketSession);
            }

            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }
    }
}
