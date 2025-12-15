using Exiled.API.Interfaces;
using System.ComponentModel;

namespace MetalDetector
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("The maximum distance to scan.")]
        public float MaxDistance { get; set; } = 5f;

        [Description("Time in seconds to wait before showing results.")]
        public float ScanDuration { get; set; } = 3f;

        [Description("Total cooldown before using again.")]
        public float Cooldown { get; set; } = 5f;

        public MetalDetectorItem MetalDetector { get; set; } = new MetalDetectorItem();
    }
}
