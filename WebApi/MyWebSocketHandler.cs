using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WebApi;

namespace WebApi
{
    public class MyWebSocketHandler : WebSocketHandler
    {
        private static WebSocketCollection clients = new WebSocketCollection();

        private static Dictionary<Guid, string> messages = new Dictionary<Guid, string>();
        private Random random = new Random();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        public CobrowsingUser User { get; set; } = new CobrowsingUser();

        public Guid ChatroomId { get; set; }
        public override void OnOpen()
        {
            try
            {
                SocketMessage socketMessage = null;
                var chatid = this.WebSocketContext.QueryString["chatroomid"];
                if (string.IsNullOrEmpty(chatid) || chatid == "undefined")
                    return;
                this.ChatroomId = Guid.Parse(chatid);

                var id = random.Next(1, 10000).ToString();
                var message = new SocketMessage() { MessageType = "1", UserId = id, ChatRoomId = chatid, Message = "connected", UserCount = "0", ScrollX = "0", ScrollY = "0", X = "0", Y = "0" };
                Send(serializer.Serialize(message));
                this.User.Id = id;
                clients.Add(this);
                var groupclients = clients.Where(cl => ((MyWebSocketHandler)cl).ChatroomId.Equals(ChatroomId));

                string lastmessage;
                if (messages.TryGetValue(this.ChatroomId, out lastmessage))
                {
                    socketMessage = serializer.Deserialize<SocketMessage>(lastmessage);
                    socketMessage.UserCount = groupclients.Count().ToString();
                    socketMessage.MessageType = "3";
                    Send(serializer.Serialize(socketMessage));

                }

                AppLogManager.LogInfo($"new user connected to {chatid} and is assigned the userid: {this.User.Id}");

            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
            //    base.Send();

        }

        public override void OnMessage(string message)
        {
            try
            {
                var chatroomid = this.WebSocketContext.QueryString["chatroomid"];
                AppLogManager.LogInfo($"chatroomid: {chatroomid} message:{message}");
                var messageObject = (SocketMessage)serializer.Deserialize(message, typeof(SocketMessage));
                if (string.IsNullOrEmpty(chatroomid) || chatroomid == "undefined")
                {
                    AppLogManager.LogInfo("chatroomid is not available");
                    return;
                }
                else
                {
                    if (messageObject.MessageType == "3")
                    {
                        if (messages.ContainsKey(Guid.Parse(chatroomid)))
                        {
                            messages[Guid.Parse(chatroomid)] = message;
                        }
                        else
                        {
                            messages.Add(Guid.Parse(chatroomid), message);
                        }
                    }
                    var groupclients = clients.Where(cl => ((MyWebSocketHandler)cl).ChatroomId.Equals(Guid.Parse(chatroomid)) && ((MyWebSocketHandler)cl).User.Id != messageObject.UserId);
                    foreach (var client in groupclients)
                    {
                        client.Send(message);
                        AppLogManager.LogInfo($"message published to {(client as MyWebSocketHandler).User.Id}");
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }

        }
        public override void OnMessage(byte[] message)
        {
            //  Debugger.Log(1, "info", message);
            var chatroomid = this.WebSocketContext.QueryString["chatroomid"];
            //Debugger.Log(1, "info", message);
            var groupclients = clients.Where(cl => ((MyWebSocketHandler)cl).ChatroomId.Equals(Guid.Parse(chatroomid)));
            (groupclients as WebSocketCollection).Broadcast(message);

        }
        public override void OnClose()
        {
            try
            {
                Close();
                AppLogManager.LogInfo("Connection closed!");
            }
            catch (Exception ex)
            {
                AppLogManager.LogError(ex);
            }
        }
    }
}