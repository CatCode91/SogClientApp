using SogClientLib.Models.Enums;
using SogClientLib.Models.Interfaces;
using System.Drawing;


namespace SogClientLib.Models
{
    public class SogMessage : ISogMessage
    {      
        public string Caption { get; set; }
        public int CaptionSize { get; set; }
        public string Text { get; set; }
        public int TextSize { get; set; }
        public MessageType Type { get; set; }
        public Image Picture { get; set; }
    }
}
