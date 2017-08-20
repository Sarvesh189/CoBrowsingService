using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi
{
    public class SocketMessage
    {
        public string UserCount { get; set; }
        public string MessageType { get; set; }

        public string Message { get; set; }

        public string ChatRoomId { get; set; }

        public string UserId { get; set; }

        public string ScrollX { get; set; }
        public string ScrollY { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
    }
}