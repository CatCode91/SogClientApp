using SogClientLib.Models;
using SogClientLib.Models.Enums;
using System;
using SogClientLib.Helpers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SogClientLib
{
    //FLOW:
    //Recieving mode - 0 for images, 1 for text
    //Recieving size of the text, then text
    //Recieving size of the caption, then caption

    public class SogListener
    {
        private SogConnection _connection;
        private Socket _socket;
        private NetworkStream _stream;
        private CancellationTokenSource _cancellationToken;
        public event Action<SogMessage> SogMessageRecieved;

        public void ConnectToServer(SogConnection connection)
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

                Task.Run(() => ListenServer(_cancellationToken.Token));
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
                int i = BitConverter.EndianBitConverter.BigEndian.ToInt32(buffer, 0);

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
            var buffer = new byte[4];
            _stream.Read(buffer, 0, 4);
            int width = BitConverter.EndianBitConverter.BigEndian.ToInt32(buffer, 0);

            if (width > 0)
            {
                buffer = new byte[4];
                _stream.Read(buffer, 0, 4);
                int height = BitConverter.EndianBitConverter.BigEndian.ToInt32(buffer, 0);
                int size = (height * width + 3) / 4;

                buffer = new byte[size];

                int readen = 0;
                while (readen < size)
                {
                    readen += _stream.Read(buffer, 0, size);
                }


                var bitmap = ImageHelper.DecodeImage(height, width, buffer);
                message.Picture = bitmap;
            }
            else 
            {
                message.Picture = null;
            }

            return message;
        }
        private SogMessage GetTextFromStream()
        {
            SogMessage message = new SogMessage();
            var buffer = new byte[4];
            _stream.Read(buffer, 0, 4);
            message.TextSize = BitConverter.EndianBitConverter.BigEndian.ToInt32(buffer, 0);

            buffer = new byte[message.TextSize];
            _stream.Read(buffer, 0, message.TextSize);
            message.Text = Encoding.UTF8.GetString(buffer);

            buffer = new byte[4];
            _stream.Read(buffer, 0, 4);
            message.CaptionSize = BitConverter.EndianBitConverter.BigEndian.ToInt32(buffer, 0);

            buffer = new byte[message.CaptionSize];
            _stream.Read(buffer, 0, message.CaptionSize);
            message.Caption = Encoding.UTF8.GetString(buffer);

            return message;
        }
    }
}
