using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;
using System.IO;

namespace Tracker
{
    class ActivitySocket
    {
        private const string socketUrl = "ws://192.168.96.175:8080/";

        private static ActivitySocket socketInstance = null;

        public static ActivitySocket GetInstance()
        {
            if (socketInstance == null)
            {
                socketInstance = new ActivitySocket();
            }
            return socketInstance;
        }

        public void send(string content)
        {
            websocket.Send(content);
        }

        public void send(Activity activity)
        {
            jsonWebsocket.Send("act", activity);
        }

        private WebSocket websocket;
        private JsonWebSocket jsonWebsocket;

        private ActivitySocket()
        {
            websocket = new WebSocket(socketUrl);
            websocket.Opened += new EventHandler(websocket_Opened);
            //websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();

            jsonWebsocket = new JsonWebSocket(socketUrl);
            jsonWebsocket.Opened += new EventHandler(json_websocket_Opened);

            jsonWebsocket.Open();
        }

        private void json_websocket_Opened(object sender, EventArgs e)
        {

        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            websocket.Send("hello");
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            Console.Write("closed");
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.Write(e.Message);
        }
    }
}
