using System.IO;
using System.IO.Pipes;

using CLROBS;

namespace CLRHostStreamStatusPlugin
{
    public class CLRHostStreamStatusPlugin : Plugin
    {
        private string _pipeName = "OBSStreamStatusPlugin_OpenBot";
        public string Description
        {
            get
            {
                return "Provides events for stream start and stop to OpenBot plugins";
            }
        }

        public string Name
        {
            get
            {
                return "OpenBot Stream Status IPC";
            }
        }

        public bool LoadPlugin()
        {
            return true;
        }

        public void OnStartStream()
        {
            SendStreamStatus(true);
        }

        public void OnStopStream()
        {
            SendStreamStatus(false);
        }

        private void SendStreamStatus(bool online)
        {
            NamedPipeClientStream client = new NamedPipeClientStream(".", _pipeName, PipeDirection.Out);
            client.Connect(1000);
            if (client.IsConnected)
            {
                client.Write(online ? new byte[] { 1 } : new byte[] { 0 }, 0, 1);
                client.Close();
                client.Dispose();
            }
        }
        public void UnloadPlugin()
        {
            SendStreamStatus(false);
        }
    }
}
