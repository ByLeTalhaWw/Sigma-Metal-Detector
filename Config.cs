using Exiled.API.Interfaces;
using System.ComponentModel;

namespace MetalDetector
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        public MetalDetectorItem MetalDetector { get; set; } = new MetalDetectorItem();
    }
}
