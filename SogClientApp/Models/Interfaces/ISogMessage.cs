using SogClientLib.Models.Enums;
using System.Drawing;

namespace SogClientLib.Models.Interfaces
{
    public interface ISogMessage
    {
        MessageType Type { get; set; }
        string Caption { get; set; }
        string Text { get; set; }
        EncodedImage EncodedImage { get; set; }
    }
}
