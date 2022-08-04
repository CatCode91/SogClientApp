using SogClientLib;
using SogClientLib.Models;
using SogClientLib.Models.Enums;
using SogClientLib.Models.Interfaces;
using SogTestForms.Helper;
using System;
using System.Windows.Forms;

namespace SogTestForms
{
    public partial class Form1 : Form
    {

        private SogListener _listener = new SogListener();
        public Form1()
        {
            InitializeComponent();
            StartListen();
        }

        public async void StartListen() 
        {
            _listener.SogMessageRecieved += _listener_NewMessage;
            await _listener.ConnectToServerAsync(new SogConnection
            {
                IpAdress = "192.168.1.177",
                Port = "8536",
                AppMode = AppMode.Picture
            });

            _listener.SogMessageRecieved += _listener_NewMessage;
        }

        private void _listener_NewMessage(ISogMessage obj)
        {
            Action action = () =>
            {
                if (obj == null)
                    return;

                if (obj.EncodedImage != null)
                {
                    picBox.Image = ImageHelper.DecodeImage(obj.EncodedImage);
                }
                else
                {
                    picBox.Image = null;
                }

                txt_m.Text = obj.Text;
            };
            Invoke(action);

        }

        private void btn_switch_Click(object sender, EventArgs e)
        {
            _listener.SwitchMode();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
