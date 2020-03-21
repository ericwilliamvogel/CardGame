using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace CardGame
{
    public enum ServerPackets
    {
        // Sent from server to client
        welcome = 1
    }
    public enum ClientPackets
    {
        // Sent from client to server
        welcomeReceived = 1,

    }
    public class ClientHandle
    {
        public static ClientHandle instance;
        private ByteBuffer buffer;
        public delegate void Packet(byte[] _data);
        public Dictionary<int, Packet> packets;

        public static ClientHandle getInstance()
        {
            if (instance == null)
            {
                instance = new ClientHandle();
            }
            else if (instance != null)
            {
                //Debug.Log("Instance already exists, destroying object!");
                //Destroy(this);
            }
            return instance;
        }

        public void InitPackets()
        {
            //Debug.Log("Initializing packets...");
            packets = new Dictionary<int, Packet>
        {
            { (int)ServerPackets.welcome, Welcome }
               // {(int)ClientPackets.welcomeReceived, ClientSend.WelcomeReceived }
        };
            //throw new Exception();
        }
        public void HandleData(byte[] _data)
        {
            byte[] _tempBuffer = (byte[])_data.Clone();
            int _packetLength = 0;
            if (buffer == null)
            {
                buffer = new ByteBuffer();
            }
            buffer.WriteBytes(_tempBuffer);
            if (buffer.Count() == 0)
            {
                buffer.Clear();
                return;
            }
            if (buffer.Length() >= 4)
            {
                _packetLength = buffer.ReadInt(false);
                if (_packetLength <= 0)
                {
                    buffer.Clear();
                    return;
                }
            }

            while (_packetLength > 0 && _packetLength <= buffer.Length() - 4)
            {
                if (_packetLength <= buffer.Length() - 4)
                {
                    buffer.ReadInt();
                    _data = buffer.ReadBytes(_packetLength);
                    HandlePackets(_data);

                }
                _packetLength = 0;
                if (buffer.Length() >= 4)
                {
                    _packetLength = buffer.ReadInt(false);
                    if (_packetLength <= 0)
                    {
                        buffer.Clear();
                        return;
                    }
                }
            }
            if (_packetLength <= 1)
            {
                buffer.Clear();
            }
        }
        private void HandlePackets(byte[] _data)
        {
            ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            int _packetID = _buffer.ReadInt();
            _buffer.Dispose();

            if (packets.TryGetValue(_packetID, out Packet _packet))
            {
                //throw new Exception();
                _packet.Invoke(_data);
            }
        }
        private static void Welcome(byte[] _data)
        {
            //throw new Exception();
            ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            _buffer.ReadInt();
            string _msg = _buffer.ReadString();
            int _myPlayerID = _buffer.ReadInt();
            _buffer.Dispose();
            File.WriteAllText(".\\DEBUG.txt", _msg);
            //throw new Exception
            ClientTCP.instance.myPlayerID = _myPlayerID;
            ClientSend.instance.WelcomeReceived();

        }
    }

    public class ClientSend
    {
        public static ClientSend instance;

        public static ClientSend getInstance()
        {
            if (instance == null)
            {
                instance = new ClientSend();
            }
            else if (instance != null)
            {
                //Debug.Log("Instance already exists, destroying object!");
                //(this);
            }
            return instance;
        }
        public void SendDataToServer(byte[] _data)
        {
            try
            {
                if (ClientTCP.instance.socket != null)
                {
                    ByteBuffer _buffer = new ByteBuffer();
                    _buffer.WriteInt(_data.GetUpperBound(0) - _data.GetLowerBound(0) + 1);
                    _buffer.WriteBytes(_data);
                    ClientTCP.instance.stream.BeginWrite(_buffer.ToArray(), 0, _buffer.ToArray().Length, null, null);

                    _buffer.Dispose();
                }
            }
            catch (Exception _ex)
            {
                //Debug.Log("Error sending data: " + _ex);
            }


        }
        public void WelcomeReceived()
        {
            ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteInt((int)ClientPackets.welcomeReceived);
            string test = "Test player name";
            _buffer.WriteString(test);
            SendDataToServer(_buffer.ToArray());

            //confirmed buffer length 24
            
            //File.WriteAllText(".\\DEBUG.txt", _buffer.ToArray().ToString());
            _buffer.Dispose();
        }
    }

    class ClientTCP
    {

        public static ClientTCP instance;
        public string ip = "127.0.0.1";
        public int port = 16320;
        public int myPlayerID = 1;
        public TcpClient socket;
        public NetworkStream stream;
        private byte[] receiveBuffer;


        public static ClientTCP getInstance()
        {
            if (instance == null)
            {
                instance = new ClientTCP();
            }
            else if (instance != null)
            {
                //Debug.Log("Instance already exists, destroying object!");
                //Destroy(this);

            }
            return instance;
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);
            if (!socket.Connected)
            {
                return;
            }
            else
            {
                socket.NoDelay = true;
                stream = socket.GetStream();
                stream.BeginRead(receiveBuffer, 0, socket.ReceiveBufferSize, ReceivedData, null);

            }
        }
        private void ReceivedData(IAsyncResult _result)
        {

            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    CloseConnection();
                    return;
                }
                byte[] _tempBuffer = new byte[_byteLength];
                Array.Copy(receiveBuffer, _tempBuffer, _byteLength);

                ClientHandle.instance.HandleData(_tempBuffer);
                stream.BeginRead(receiveBuffer, 0, socket.ReceiveBufferSize, ReceivedData, null);
            }
            catch (Exception _ex)
            {
                //Debug.Log("Error receiving data: " + _ex);
                CloseConnection();
                return;
            }
        }
        private void CloseConnection()
        {
            socket.Close();
        }


        public void Start()
        {
            ConnectToServer();
        }
        public void ConnectToServer()
        {
            ClientHandle.instance.InitPackets();

            socket = new TcpClient
            {
                ReceiveBufferSize = 4096,
                SendBufferSize = 4096,
                NoDelay = false
            };
            receiveBuffer = new byte[socket.ReceiveBufferSize];
            socket.BeginConnect(ip, port, ConnectCallback, socket);
        }

    }
}
