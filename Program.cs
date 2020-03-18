using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;

namespace CardGameServer
{
    /*
     * When creating packets, always stick to this format:

    create a ByteBuffer instance
    write the packet ID to the buffer
    write any data you wish to send to the buffer
    send the buffer
    dispose the buffer

    */
    
    class Player
    {
        public int ID;
        public Player(int _ID)
        {
            ID = _ID;
        }
    }
    class ServerSend
    {
        public static void SendDataTo(int _playerID, byte[] _data)
        {
            try
            {
                if (Globals.clients[_playerID].socket != null)
                {
                    ByteBuffer _buffer = new ByteBuffer();
                    _buffer.WriteInt(_data.GetUpperBound(0) - _data.GetLowerBound(0) + 1);
                    _buffer.WriteBytes(_data);

                    Globals.clients[_playerID].stream.BeginWrite(_buffer.ToArray(), 0, _buffer.ToArray().Length, null, null);
                    _buffer.Dispose();
                }

            }
            catch (Exception _ex)
            {
                Logger.Log(LogType.error, "Error sending data to player " + _playerID + ": " + _ex);
            }
        }

        public static void Welcome(int _sendToPlayer, string _msg)
        {
            ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteInt((int)ServerPackets.welcome);

            _buffer.WriteString(_msg);
            _buffer.WriteInt(_sendToPlayer);

            SendDataTo(_sendToPlayer, _buffer.ToArray());
            _buffer.Dispose();

        }
    }
    class ServerHandle
    {
        public delegate void Packet(int _playerID, byte[] _data);
        public static Dictionary<int, Packet> packets;


        public static void HandleData(int _playerID, byte[] _data)
        {
            byte[] _tempBuffer = (byte[])_data.Clone();
            int _packetLength = 0;

            if (Globals.clients[_playerID].buffer == null)
            {
                Globals.clients[_playerID].buffer = new ByteBuffer();
            }

            if (Globals.clients[_playerID].buffer.Length() >= 4)
            {
                _packetLength = Globals.clients[_playerID].buffer.ReadInt(false);
                if (_packetLength <= 0)
                {
                    Globals.clients[_playerID].buffer.Clear();
                    return;
                }
            }

            while (_packetLength > 0 && _packetLength <= Globals.clients[_playerID].buffer.Length() - 4)
            {
                Globals.clients[_playerID].buffer.ReadInt();
                _data = Globals.clients[_playerID].buffer.ReadBytes(_packetLength);
                HandlePackets(_playerID, _data);

                _packetLength = 0;
                if (Globals.clients[_playerID].buffer.Length() >= 4)
                {
                    _packetLength = Globals.clients[_playerID].buffer.ReadInt(false);
                    if (_packetLength <= 0)
                    {
                        Globals.clients[_playerID].buffer.Clear();
                        return;
                    }
                }

            }

            if (_packetLength <= 1)
            {
                Globals.clients[_playerID].buffer.Clear();
            }


        }

        private static void HandlePackets(int _playerID, byte[] _data)
        {
            ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            int _packetID = _buffer.ReadInt();
            _buffer.Dispose();

            if (packets.TryGetValue(_packetID, out Packet _packet))
            {
                _packet.Invoke(_playerID, _data);
            }

        }

        public static void InitPackets()
        {
            Logger.Log(LogType.info1, "Initializing packets..");
            packets = new Dictionary<int, Packet>()
            {
                { (int)ClientPackets.welcomeReceived, WelcomeReceived }
            };
        }

        private static void WelcomeReceived(int _playerID, byte[] _data)
        {
            ByteBuffer _buffer = new ByteBuffer();
            _buffer.WriteBytes(_data);
            _buffer.ReadInt();
            string _username = _buffer.ReadString();
            _buffer.Dispose();
            Logger.Log(LogType.info2, "Connection from " + Globals.clients[_playerID].socket.Client.RemoteEndPoint + " was successful. Username: " + _username);
        }
    }
    class Client
    {
        public int playerID;
        public bool isPlaying = false;
        public TcpClient socket;
        public NetworkStream stream;
        public ByteBuffer buffer;
        public Player player;
        private byte[] receiveBuffer;

        public void StartClient()
        {
            socket.ReceiveBufferSize = 4096;
            socket.SendBufferSize = 4096;
            stream = socket.GetStream();
            receiveBuffer = new byte[socket.ReceiveBufferSize];
            stream.BeginRead(receiveBuffer, 0, socket.ReceiveBufferSize, ReceivedData, null);
            player = new Player(playerID);
            ServerSend.Welcome(playerID, "Welcome to the server!");
        }
        private void CloseConnection()
        {
            Logger.Log(LogType.info1, "Connection from " + socket.Client.RemoteEndPoint.ToString() + " has been terminated");
            player = null;
            isPlaying = false;
            socket.Close();
            socket = null;
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
                ServerHandle.HandleData(playerID, _tempBuffer); // We'll create this later
                stream.BeginRead(receiveBuffer, 0, socket.ReceiveBufferSize, ReceivedData, null);

            }
            catch (Exception _ex)
            {
                Logger.Log(LogType.error, "Error while receiving data: " + _ex);
                CloseConnection();

                return;
            }


        }
    }
    class Globals
    {
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public static bool serverIsRunning = false;
    }
    class Constants
    {
        public const int MAX_PLAYERS = 100;
        public const int TICKS_PER_SEC = 30;
        public const float MS_PER_TICK = 1000 / TICKS_PER_SEC;
    }
    class General
    {

        public static void StartServer()
        {
            InitServerData();
            ServerTCP.InitNetwork();
            Logger.Log(LogType.info2, "Server started");
        }

        private static void InitServerData()
        {
            for (int i = 1; i <= Constants.MAX_PLAYERS; i++)
            {
                Globals.clients.Add(i, new Client());
            }
        }

    }
        class ServerTCP
    {
        private static TcpListener socket;
        private static int port = 16320;
        public static void InitNetwork()
        {
            Logger.Log(LogType.info1, "Starting server on port " + port + "..");
            ServerHandle.InitPackets();
            socket = new TcpListener(IPAddress.Any, port);
            socket.Start();
            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);

        }
        private static void ClientConnected(IAsyncResult _result)
        {
            TcpClient _client = socket.EndAcceptTcpClient(_result);
            _client.NoDelay = false;
            socket.BeginAcceptTcpClient(new AsyncCallback(ClientConnected), null);
            Logger.Log(LogType.info1, "Incoming connection from " + _client.Client.RemoteEndPoint.ToString());

            for (int i = 1; i <= Constants.MAX_PLAYERS; i++)
            {
                if (Globals.clients[i].socket == null)
                {
                    Globals.clients[i].socket = _client;
                    Globals.clients[i].playerID = i;
                    Globals.clients[i].StartClient();
                    return;
                }
            }

            Logger.Log(LogType.warning, "Server full");
        }
    }
    class Program
    {
        //private TcpListener myListener;
        //private int port = 5050;
        static void Main(string[] args)
        {
            Globals.serverIsRunning = true;
            Logger.Initialize(ConsoleColor.Cyan);
            Thread _gameThread = new Thread(new ThreadStart(GameLogicThread));
            _gameThread.Start();

            General.StartServer();

        }
        private static void GameLogicThread()
        {
            Logger.Log(LogType.info1, "Game thread started. Running at " + Constants.TICKS_PER_SEC + " ticks per second");
            // Game logic would go here
            DateTime _lastLoop = DateTime.Now;
            DateTime _nextLoop = _lastLoop.AddMilliseconds(Constants.MS_PER_TICK);
            while (Globals.serverIsRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    Logger.WriteLogs();
                    _lastLoop = _nextLoop;
                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (_nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(_nextLoop - DateTime.Now);
                    }

                }
            }

        }
    }
}
