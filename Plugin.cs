using Exiled.API.Features;
using System;

namespace MetalDetector
{
    public class Plugin : Plugin<Config, Translations>
    {
        public override string Author => "ByLeTalha";
        public override string Name => "MetalDetector";
        public override string Prefix => "SigmaMetal";
        public override Version Version => new Version(2, 0, 0);

        public static Plugin Instance;

        public override void OnEnabled()
        {
            Instance = this;
            Exiled.CustomItems.API.Features.CustomItem.RegisterItems(overrideClass: Config);
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.CustomItems.API.Features.CustomItem.UnregisterItems();
            Instance = null;
            base.OnDisabled();
        }
    }
}
