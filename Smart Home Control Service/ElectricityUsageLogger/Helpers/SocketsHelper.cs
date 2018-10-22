using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SmartHomeControl.Helpers {

    public delegate void BytesReceivedDelegate(string receivedBuffer);

    public class SocketsHelper {
        private static List<HeartbeatInfo> openSocketsList = new List<HeartbeatInfo>();

        private static Socket CheckIfSocketAlreadyOpen(IPEndPoint source, IPEndPoint destination) {
            foreach (HeartbeatInfo info in openSocketsList) {
                if (((IPEndPoint)info.DestinationSocket.LocalEndPoint).Address.Equals(source.Address) &&
                    ((IPEndPoint)info.DestinationSocket.RemoteEndPoint).Address.Equals(destination.Address) &&
                    ((IPEndPoint)info.DestinationSocket.RemoteEndPoint).Port == destination.Port) {
                        return info.DestinationSocket;
                }
            }
            return null;
        }

        public static void SendMessageToTCPSocket(IPEndPoint source, IPEndPoint destination, byte[] bufferToSend) {
            SendMessageToSocketAndListenToCallback(source, destination, bufferToSend, null, ProtocolType.Tcp);
        }

        public static void SendMessageToUDPSocket(IPEndPoint source, IPEndPoint destination, byte[] bufferToSend) {
            SendMessageToSocketAndListenToCallback(source, destination, bufferToSend, null, ProtocolType.Udp);
        }

        public static void SendMessageToTCPSocketAndListenToCallback(IPEndPoint source, IPEndPoint destination, byte[] bufferToSend, BytesReceivedDelegate callBack) {
            SendMessageToSocketAndListenToCallback(source, destination, bufferToSend, callBack, ProtocolType.Tcp);
        }

        public static void SendMessageToTCPSocketAndListenToCallback(IPEndPoint source, IPEndPoint destination, byte[] bufferToSend, BytesReceivedDelegate callBack, byte[] heartbeatMessage, Regex responseFormat, BytesReceivedDelegate heartbeatCallback) {
            SendMessageToSocketAndListenToCallback(source, destination, bufferToSend, callBack, ProtocolType.Tcp, heartbeatMessage, responseFormat, heartbeatCallback, false, true);
        }

        public static void SendMessageToUDPSocketAndListenToCallback(IPEndPoint source, IPEndPoint destination, byte[] bufferToSend, BytesReceivedDelegate callBack) {
            SendMessageToSocketAndListenToCallback(source, destination, bufferToSend, callBack, ProtocolType.Udp);
        }

        public static void SendMessageToUDPSocketAndListenToCallback(IPEndPoint source, IPEndPoint destination, byte[] bufferToSend, BytesReceivedDelegate callBack, byte[] heartbeatMessage, Regex responseFormat, BytesReceivedDelegate heartbeatCallback) {
            SendMessageToSocketAndListenToCallback(source, destination, bufferToSend, callBack, ProtocolType.Udp, heartbeatMessage, responseFormat, heartbeatCallback, false, true);
        }

        public static void ConnectToTCPSocketAndListenToCallback(IPEndPoint source, IPEndPoint destination, BytesReceivedDelegate callBack) {
            SendMessageToTCPSocketAndListenToCallback(source, destination, null, callBack);
        }

        public static void ConnectToTCPSocketAndListenToCallback(IPEndPoint source, IPEndPoint destination, BytesReceivedDelegate callBack, byte[] heartbeatMessage, Regex responseFormat, BytesReceivedDelegate heartbeatCallback) {
            SendMessageToTCPSocketAndListenToCallback(source, destination, null, callBack, heartbeatMessage, responseFormat, heartbeatCallback);
        }

        public static void SendMessageToSocketAndListenToCallback(IPEndPoint source, IPEndPoint destination, byte[] bufferToSend, BytesReceivedDelegate callBack, ProtocolType protocol) {
            SendMessageToSocketAndListenToCallback(source, destination, bufferToSend, callBack, protocol, null, null, null, false, true);
        }

        public static byte[] SendMessageToSocketAndReadSynchroniously(IPEndPoint source, IPEndPoint destination, byte[] bufferToSend, ProtocolType protocol) {
            return SendMessageToSocketAndListenToCallback(source, destination, bufferToSend, null, ProtocolType.Tcp, null, null, null, true, true);
        }

        public static byte[] SendMessageToSocketAndReadSynchroniously(IPEndPoint source, IPEndPoint destination, byte[] bufferToSend, ProtocolType protocol, bool useSocketPool)
        {
            return SendMessageToSocketAndListenToCallback(source, destination, bufferToSend, null, ProtocolType.Tcp, null, null, null, true, false);
        }

        private static byte[] SendMessageToSocketAndListenToCallback(IPEndPoint source, IPEndPoint destination, byte[] bufferToSend, BytesReceivedDelegate callBack, ProtocolType protocol, byte[] heartbeatMessage, Regex responseFormat, BytesReceivedDelegate heartbeatCallback, bool isSynchronious, bool useSocketPool) {
            Socket TCPSocket = null;
            if (useSocketPool) {
                TCPSocket = CheckIfSocketAlreadyOpen(source, destination);
            }
            HeartbeatInfo info = null;
            try {
                if (TCPSocket == null) {
                    if (protocol == ProtocolType.Udp) {
                        TCPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, protocol);
                    }
                    else if (protocol == ProtocolType.Tcp) {
                        TCPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocol);
                    }
                 
                    TCPSocket.Bind(source);
                    if (protocol == ProtocolType.Tcp) {
                        TCPSocket.Connect(destination);
                        if (useSocketPool) {
                            info = new HeartbeatInfo(TCPSocket, heartbeatMessage, responseFormat, heartbeatCallback, 10);
                            info.CallbackDelegate += new BytesReceivedDelegate(CheckIfHeartbeatFailed);
                            lock (openSocketsList)
                            {
                                openSocketsList.Add(info);
                            }
                        }
                    }
                }
//                lock (TCPSocket) {
                    if (bufferToSend != null) {
                        if (protocol == ProtocolType.Tcp) {
                            TCPSocket.Send(bufferToSend);
                        }
                        else if (protocol == ProtocolType.Udp) {
                            TCPSocket.SendTo(bufferToSend, destination);
                        }
                    }

                    StateObject state = new StateObject();
                    if (isSynchronious) {
                        TCPSocket.ReceiveTimeout = 5000;
                        TCPSocket.Receive(state.buffer);
                        if (!useSocketPool) {
                            TCPSocket.Shutdown(SocketShutdown.Both);
                            TCPSocket.Close();
                        }
                        return state.buffer;
                    }
                    else if (callBack != null) {
                        state.workSocket = TCPSocket;
                        state.callbackDelegate += callBack;
                        if (info != null) {
                            state.callbackDelegate += info.ProcessMessageReceivedFromSocket;
                        }
                        TCPSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReadCallback), state);
                    }
 //               }
                return null;
            }
            catch (Exception ex) {
                LoggingHelper.LogExceptionInApplicationLog(ex.Source, ex, EventLogEntryType.Error);
                LoggingHelper.WriteExceptionLogEntry(ex.Source, ex);

                if (TCPSocket != null && TCPSocket.Connected) {
                    TCPSocket.Shutdown(SocketShutdown.Both);
                    TCPSocket.Close();
                }
            }
            return null;
        }

        private static void RemoveMalfunctioningSocket(Socket socket) {
            HeartbeatInfo hiFound = null;
            foreach (HeartbeatInfo hi in openSocketsList) {
                if (hi.DestinationSocket == socket) {
                    hiFound = hi;
                    break;
                }
            }
            if (hiFound != null) {
                lock (openSocketsList) {
                    openSocketsList.Remove(hiFound);
                }
            }

        }

        private static void ReadCallback(IAsyncResult ar) {
            String content = String.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            lock (state.workSocket) {
                Socket handler = state.workSocket;
                int bytesRead = 0;
                try {
                    bytesRead = handler.EndReceive(ar);
                } catch {
                    RemoveMalfunctioningSocket(handler);
                }

                if (bytesRead > 0) {
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));
                    content = state.sb.ToString();
                    state.callbackDelegate(content);
                }

                StateObject state1 = new StateObject();
                state1.workSocket = handler;
                state1.callbackDelegate = state.callbackDelegate;

                handler.BeginReceive(state1.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state1);
            }
        }

        private static void CheckIfHeartbeatFailed(string receivedMessage) {
            if (receivedMessage == String.Empty) {
                lock (openSocketsList) {
                    int maxCount = openSocketsList.Count;
                    for (int i = 0; i < maxCount; i++) {
                        if (openSocketsList[i].DestinationSocket == null) {
                            openSocketsList.RemoveAt(i);
                            i--;
                            maxCount--;
                        }
                    }
                }
            }
        }
    }
}
