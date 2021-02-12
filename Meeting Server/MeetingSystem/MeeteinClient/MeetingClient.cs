﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Net;
using MeetingSystem.IO;
using System.Collections;
using MeetingSystem.Message;

namespace MeetingSystem.Client
{
    public partial class MeetingClient : Client
    {
        private Thread dataListenThread = null;
        
        public MeetingClient(TcpClient tcpClient)
        {
            try
            {
                this.tcpClient = tcpClient;
                this.writer = new BEBinaryWriter(tcpClient.GetStream());
                this.reader = new BEBinaryReader(tcpClient.GetStream());
            }
            catch (Exception e)
            {
                notifyClientExceptionOcurred(this,e);
                EventLog.Write(e.ToString());
            }
        }

        public override void send(ClientMessage clientMessage, Object data)
        {
            IMessage message = MessagePool.getMessage(clientMessage);
            message.sendMessage(this, writer, data);
        }

        public override void close()
        {
            try
            {
                if (dataListenThread != null)
                {
                    dataListenThread.Interrupt();
                    dataListenThread = null;
                }

                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient = null;
                }
            }
            catch (Exception e)
            {
                notifyClientExceptionOcurred(this, e);
                EventLog.Write("stop " + e.ToString());
            }
        }

        public override void startListen()
        {
            State = ClientState.Init;
            if (dataListenThread == null)
            {
                dataListenThread = new Thread(new ThreadStart(dataReceive));
                dataListenThread.Start();
            }
            else
                throw new Exception("Data listener aleardy running.");
        }

        private void dataReceive()
        {
            try
            {
                State = ClientState.Running;
                Console.WriteLine(IpAddr.ToString() + " : DataListenThread started.");
                EventLog.Write(IpAddr.ToString() + " : DataListenThread started.");

                while (dataListenThread != null && dataListenThread.ThreadState == ThreadState.Running)
                {
                    if (reader != null)
                    {
                        int cmdVal = reader.ReadInt32();
                        EventLog.Write(IpAddr.ToString() + " cmdVal: " + cmdVal);

                        if (Enum.IsDefined(typeof(ClientMessage), cmdVal))
                        {
                            ClientMessage cmd = (ClientMessage)cmdVal;
                            IMessage message = MessagePool.getMessage(cmd);
                            if (message != null)
                            {
                                // EventLog.Write(IpAddr.ToString() + " reader string " + reader.ReadString());
                                EventLog.Write(IpAddr.ToString() + " Message is not null " + message);
                                Object obj = message.receivedMessage(this, reader);
                                message.handlerMessage(this, writer, obj);
                            }
                            else
                                throw new Exception(IpAddr.ToString() + " : Message " + Enum.GetName(typeof(ClientMessage), cmd) + " not defined");
                        }
                        else
                            throw new Exception(IpAddr.ToString() + " : Undefined command value " + cmdVal);
                    }
                    else
                        throw new Exception(IpAddr.ToString() + " : Reader is a null pointer");
                }

                Console.WriteLine(IpAddr.ToString() + " : DataListenThread stoped.");
                EventLog.Write(IpAddr.ToString() + " : DataListenThread stoped.");
            }
            catch (Exception e)
            {
                notifyClientExceptionOcurred(this, e);
                EventLog.Write(IpAddr.ToString() + " dataReceive " + e.GetType() + "\n" + e + "\n" + e.StackTrace);
            }
        }
    }
}
