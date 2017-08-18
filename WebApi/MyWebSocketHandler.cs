using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using WebApi;

namespace WebApi
{
    public class MyWebSocketHandler: WebSocketHandler
    {
        private static WebSocketCollection clients = new WebSocketCollection();

        public override void OnOpen()
        {
           
            clients.Add(this);
          //  base.Send("You are connected to a WebSocket your turn: "+clients.Count);

        }

        public override void OnMessage(string message)
        {
            Debugger.Log(1, "info", message);
            foreach (var client in clients)
            {
                client.Send(message);
            }
        }
        public override void OnMessage(byte[] message)
        {
          //  Debugger.Log(1, "info", message);
            foreach (var client in clients)
            {
                client.Send(message);
            }
        }
        public override void OnClose()
        {
            Close();
        }
    }
}