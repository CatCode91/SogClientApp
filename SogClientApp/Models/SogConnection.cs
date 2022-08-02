using SogClientLib.Models.Enums;

namespace SogClientLib.Models
{
    public class SogConnection
    {
        public string IpAdress { get;set; }
        public string Port { get; set; }
        public AppMode AppMode  { get; set; }
    }
}
