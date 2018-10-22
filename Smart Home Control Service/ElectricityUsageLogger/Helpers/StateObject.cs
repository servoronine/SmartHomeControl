using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SmartHomeControl.Helpers {
    public class StateObject {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
        //Callback method
        public BytesReceivedDelegate callbackDelegate;

        public void ClearBufferContents() {
            for (int i = 0; i < BufferSize; i++) {
                buffer[i] = 0;
            }
            sb.Clear();
        }
    }
}
