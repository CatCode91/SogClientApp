using SogClientLib.Models;
using SogClientLib.Models.Enums;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SogClientLib.Models.Interfaces;

namespace SogClientLib
{
    //FLOW:
    //Recieving mode - 0 for images, 1 for text
    //Recieving size of the text, then text
    //Recieving size of the caption, then caption

    [Serializable]
    public class SogListener
    {
        private SogConnection _connection;
        private Socket _socket;
        private NetworkStream _stream;
        private CancellationTokenSource _cancellationToken;

        public bool IsAvailiable => _socket.Connected;
        public AppMode Mode => _connection.AppMode;
        public event Action<ISogMessage> SogMessageRecieved;

        public async Task ConnectToServerAsync(SogConnection connection)
        {
            _connection = connection;
            int port = int.Parse(_connection.Port);

            IPEndPoint localIPEndPoint = new IPEndPoint(IPAddress.Parse(_connection.IpAdress), port);
            _cancellationToken = new CancellationTokenSource();

            if (_socket == null)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    _socket.Connect(localIPEndPoint);
                    if (_socket != null)
                    {
                        byte[] message = new byte[2];
                        message[0] = 100;
                        message[1] = (byte)_connection.AppMode;
                        _socket.Send(message);
                    }
                }

                catch (Exception ex)
                {
                    _socket = null;
                    throw ex;
                }
                _stream = new NetworkStream(_socket);

                await Task.Run(() => ListenServer(_cancellationToken.Token));
            }
        }
        public void SwitchMode(AppMode? appMode = null)
        {
            if (_connection == null)
                throw new Exception("You have to connect to server first!");
            
            if (appMode == null)
                _connection.AppMode = _connection.AppMode == 0 ? AppMode.Picture : AppMode.Text;

            if (_socket != null)
            {
                byte[] message = new byte[2];
                message[0] = 100;
                message[1] = (byte)((appMode == AppMode.Picture) ? 0 : 1);
                _socket.Send(message);
            }
        }
        private void ListenServer(CancellationToken token)
        {
            SogMessage message = null;

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                byte[] buffer = new byte[4];
                _stream.Read(buffer, 0, 4);
                int i = BinaryEncoding.Binary.BigEndian.GetInt32(buffer, 0);

                switch (i)
                {
                    case (0):
                        message =  GetImageFromStream();
                        break;
                    case (1):
                        message = GetTextFromStream();
                        break;
                    default:
                        break;
                }

                SogMessageRecieved?.Invoke(message);
            }
        }
        private SogMessage GetImageFromStream()
        {
            SogMessage message = new SogMessage();
            message.Type = MessageType.Picture;
            var buffer = new byte[4];
            _stream.Read(buffer, 0, 4);
            int width = BinaryEncoding.Binary.BigEndian.GetInt32(buffer, 0);

            if (width > 0)
            {
                buffer = new byte[4];
                _stream.Read(buffer, 0, 4);
                int height = BinaryEncoding.Binary.BigEndian.GetInt32(buffer, 0);
                int size = (height * width + 3) / 4;

                buffer = new byte[size];

                int readen = 0;
                while (readen < size)
                {
                    readen += _stream.Read(buffer, 0, size);
                }

                message.EncodedImage = new EncodedImage 
                { 
                    Height = height,
                    Width = width,
                    ByteArray = buffer
                };
            }
            else 
            {
                message.EncodedImage = null;
            }

            return message;
        }
        private SogMessage GetTextFromStream()
        {
            SogMessage message = new SogMessage();
            message.Type = MessageType.Text;
            var buffer = new byte[4];
            _stream.Read(buffer, 0, 4);
            message.TextSize = BinaryEncoding.Binary.BigEndian.GetInt32(buffer, 0);

            buffer = new byte[message.TextSize];
            _stream.Read(buffer, 0, message.TextSize);
            message.Text = Encoding.UTF8.GetString(buffer);

            buffer = new byte[4];
            _stream.Read(buffer, 0, 4);
            message.CaptionSize = BinaryEncoding.Binary.BigEndian.GetInt32(buffer, 0);

            buffer = new byte[message.CaptionSize];
            _stream.Read(buffer, 0, message.CaptionSize);
            message.Caption = Encoding.UTF8.GetString(buffer);

            return message;
        }
    }
}
