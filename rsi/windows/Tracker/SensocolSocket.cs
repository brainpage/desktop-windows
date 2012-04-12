﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;
using SuperSocket.ClientEngine;
using Newtonsoft.Json;
using System.Timers;

namespace Tracker
{
    class SensocolSocket
    {
        private static SensocolSocket socketInstance = null;
        public static SensocolSocket GetInstance()
        {
            if (socketInstance == null)
            {
                socketInstance = new SensocolSocket();
            }
            return socketInstance;
        }

        private const string CONNECTED = "connected";
        private const string DISCONNECTED = "disconnected";

        private WebSocket websocket;
        private string state;
        private List<Dictionary<string, object>> eventQueue;
        private Timer sendTimer;
        private Timer reconnectTimer;
        private int ackCounter;

        private SensocolSocket()
        {
            OpenNewSocket();

            sendTimer = new Timer();
            sendTimer.Interval = 2000;
            sendTimer.Elapsed += new ElapsedEventHandler(this.sendTimer_Tick);
            sendTimer.Start();

            reconnectTimer = new Timer();
            reconnectTimer.Interval = AppConfig.ReconnectMinInterval;
            reconnectTimer.Elapsed += new ElapsedEventHandler(this.reconnectTimer_Tick);
            reconnectTimer.Start();

            state = DISCONNECTED;

            eventQueue = (List<Dictionary<string, object>>)Utils.ReadFile("events");
            if (eventQueue == null)
                eventQueue = new List<Dictionary<string, object>>();

            ackCounter = 0;
        }

        private void OpenNewSocket()
        {
            websocket = new WebSocket(AppConfig.SocketUrl);
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();
        }

        private void sendTimer_Tick(object sender, ElapsedEventArgs e)
        {
            SendQueue();
        }

        private void reconnectTimer_Tick(object sender, ElapsedEventArgs e)
        {
            OpenNewSocket();

            //Extend reconnect interval
            if (reconnectTimer.Interval < AppConfig.ReconnectMaxInterval)
                reconnectTimer.Interval *= 2;
        }

        private void Reconnect()
        {
            state = DISCONNECTED;

            if (!reconnectTimer.Enabled)
            {
                reconnectTimer.Interval = AppConfig.ReconnectMinInterval;
                reconnectTimer.Enabled = true;
            }
        }

        public void SendEvent(string eventName, Dictionary<string, object> data)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime dt = DateTime.Now;
            long timestamp = (long)(dt - unixEpoch).TotalSeconds;

            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add("code", eventName);
            eventData.Add("data", data);
            eventData.Add("action", "event");
            eventData.Add("timestamp", timestamp);

            eventQueue.Add(eventData);
            if (eventQueue.Count > AppConfig.MaxQueueCount)
                eventQueue.RemoveAt(0);
        }

        public void RequestLoginTokenFor(string url)
        {
            Dictionary<string, object> data = new Dictionary<string,object>();
            data.Add("action", "sys_cmd");
            data.Add("command", "user_login_token");
            data.Add("ack",GetAck());
            data.Add("url", url);

            this.Send(data);
        }

        public void CacheQueueToFile()
        {
            Utils.WriteFile("events", eventQueue);
        }

        private void Send(Object content)
        {
            websocket.Send(content.GetType().Name == "String" ? content.ToString() : JsonConvert.SerializeObject(content));
        }

        private void SendQueue()
        {
            if (state == CONNECTED)
            {
                if (eventQueue.Count == 0)
                    return;

                if (eventQueue.Count == 1)
                {
                    this.Send(eventQueue[0]);
                    eventQueue.RemoveAt(0);
                }
                else
                {
                    List<Dictionary<string, object>> events = new List<Dictionary<string, object>>();
                    int totalBytes = 0;
                    Dictionary<string, object> data;
                    while (eventQueue.Count > 0)
                    {
                        data = eventQueue[0];
                        eventQueue.RemoveAt(0);
                        events.Add(data);

                        ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                        totalBytes += encoding.GetBytes(JsonConvert.SerializeObject(data)).Length;
                        if (totalBytes > AppConfig.MaxBatchSize)
                            break;
                    }

                    Dictionary<string, object> batchEvent = new Dictionary<string, object>();
                    batchEvent.Add("events", events);
                    batchEvent.Add("action", "event_batch");
                    this.Send(batchEvent);
                }
            }
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            Connect();
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            Reconnect();
        }

        private void websocket_Error(object sender, ErrorEventArgs e)
        {
            Reconnect();
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                Dictionary<string, string> response = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Message);
                switch (state)
                {
                    case CONNECTED:
                        if (response["action"] == "sys_cmd_re" && response["command"] == "user_login_token")
                        {
                            AppData.GetInstance().SetLoginUrl(response["url"]);
                        }
                        if (response["action"] == "command" && response["command"] == "rsi_alert")
                        {
                            FormState.GetInstance().BeginNotify();
                        }
                        break;
                    case DISCONNECTED:
                        if (response["action"] == "connack" && response["status"] == "ok")
                        {
                            state = CONNECTED;
                            reconnectTimer.Enabled = false;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }

        private void Connect()
        {
            Dictionary<string, string> sensor = new Dictionary<string, string>();
            sensor.Add("uuid", AppData.GetInstance().SensorUUID);
            sensor.Add("token", AppData.GetInstance().AuthToken);
            sensor.Add("description", System.Environment.MachineName);

            Dictionary<string, object> connect = new Dictionary<string, object>();
            connect.Add("action", "connect");
            connect.Add("sensor", sensor);

            this.Send(connect);
        }

        private int GetAck()
        {
            return ackCounter++;
        }
    }
}
