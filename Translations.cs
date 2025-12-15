using Exiled.API.Interfaces;
using System.ComponentModel;

namespace MetalDetector
{
    public class Translations : ITranslation
    {
        [Description("Message shown to the scanner when they start scanning.")]
        public string ScanStarted { get; set; } = "Scanning %player%...";

        [Description("Message shown to the player being scanned.")]
        public string TargetScanned { get; set; } = "You are being scanned by a Metal Detector.";

        [Description("Header for the scan result.")]
        public string ScanResult { get; set; } = "<color=yellow>Items found on %player%:</color>";

        [Description("Message shown when the target has no items.")]
        public string NoItems { get; set; } = "%player% has no items.";

        [Description("Message shown when no player is found.")]
        public string NoPlayerFound { get; set; } = "No player found.";

        [Description("Message shown when scanner is in cooldown.")]
        public string CooldownMessage { get; set; } = "Metal Detector is cooling down.";
    }
}
