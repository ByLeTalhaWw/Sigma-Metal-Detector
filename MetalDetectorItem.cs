using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Items;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;

namespace MetalDetector
{
    [CustomItem(ItemType.GunCOM15)]
    public class MetalDetectorItem : CustomWeapon
    {
        public override uint Id { get; set; } = 60;
        public override string Name { get; set; } = "Metal Detector";
        public override string Description { get; set; } = "Scans players for items.";
        public override float Weight { get; set; } = 1f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint
                {
                    Chance = 100,
                    Location = Exiled.API.Enums.SpawnLocationType.InsideLczArmory,
                }
            }
        };

        public override float Damage { get; set; } = 0;
        public override byte ClipSize { get; set; } = 12;

        private Dictionary<int, float> _cooldowns = new Dictionary<int, float>();
        private readonly int _mask = LayerMask.GetMask("Default", "Player", "Hitbox");

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Left += OnLeft;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            base.UnsubscribeEvents();
        }

        protected override void OnShooting(ShootingEventArgs ev)
        {
            ev.IsAllowed = false;

            if (_cooldowns.TryGetValue(ev.Player.Id, out float nextUseTime) && Time.time < nextUseTime)
            {
                ev.Player.ShowHint(Plugin.Instance.Translation.CooldownMessage, 2f);
                return;
            }

            Vector3 startPos = ev.Player.CameraTransform.position + (ev.Player.CameraTransform.forward * 0.1f);

            // Use Config.MaxDistance
            if (Physics.Raycast(startPos, ev.Player.CameraTransform.forward, out RaycastHit hit, Plugin.Instance.Config.MaxDistance, _mask))
            {
                Player target = Player.Get(hit.collider);

                if (target != null && target != ev.Player && !target.IsScp)
                {
                    // Use Config.Cooldown
                    _cooldowns[ev.Player.Id] = Time.time + Plugin.Instance.Config.Cooldown;
                    Timing.RunCoroutine(ScanRoutine(ev.Player, target));
                }
                else
                {
                    ev.Player.ShowHint(Plugin.Instance.Translation.NoPlayerFound, 2f);
                }
            }
            else
            {
                ev.Player.ShowHint(Plugin.Instance.Translation.NoPlayerFound, 2f);
            }
        }

        private IEnumerator<float> ScanRoutine(Player scanner, Player target)
        {
            scanner.ShowHint(Plugin.Instance.Translation.ScanStarted.Replace("%player%", target.Nickname), 2f);
            target.Broadcast(3, Plugin.Instance.Translation.TargetScanned);

            // Use Config.ScanDuration
            yield return Timing.WaitForSeconds(Plugin.Instance.Config.ScanDuration);

            if (scanner == null || target == null || !scanner.IsConnected || !target.IsConnected)
                yield break;

            if (target.Items.Count == 0)
            {
                scanner.ShowHint(Plugin.Instance.Translation.NoItems.Replace("%player%", target.Nickname), 4f);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(Plugin.Instance.Translation.ScanResult.Replace("%player%", target.Nickname));
                
                foreach (Item item in target.Items)
                {
                    sb.AppendLine($"- {item.Type}");
                }

                scanner.ShowHint(sb.ToString(), 6f);
            }
        }

        private void OnLeft(LeftEventArgs ev)
        {
            if (_cooldowns.ContainsKey(ev.Player.Id))
                _cooldowns.Remove(ev.Player.Id);
        }
    }
}
