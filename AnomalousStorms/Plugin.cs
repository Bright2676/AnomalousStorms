/*
____ _       _     _   
 |  _ \     (_)     | |   | |  
 | |_) |_ __ _  __ _| |__ | |_ 
 |  _ <| '__| |/ _` | '_ \| __|
 | |_) | |  | | (_| | | | | |_ 
 |____/|_|  |_|\__, |_| |_|\__|
                __/ |          
               |___/           
*/

//authors: bright2676

using AnomalousStorms.Events;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace AnomalousStorms
{
    public class Plugin : Plugin<Config>
    {
        public override string Author => "Bright2676";
        public override string Name => "AnomalousStorms";
        public override string Prefix => "anomalous_storms";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 8, 1);

        private static readonly Plugin Singleton = new Plugin();
        private static readonly Random rng = new Random();

        private ServerHandler server_handler;
        private PlayerHandler player_handler;

        public static Plugin Instance => Singleton;

        private static readonly EffectType[] PossibleEffects =
        {
            EffectType.Blinded,
            EffectType.Asphyxiated,
            EffectType.CardiacArrest,
            EffectType.Concussed,
            EffectType.Deafened,
            EffectType.Poisoned,
            EffectType.SinkHole,
            EffectType.Stained,
            EffectType.Traumatized,
            EffectType.Invigorated
        };

        private Plugin() { }
        private void RegisterEvents()
        {
            server_handler = new ServerHandler();
            player_handler = new PlayerHandler();

            Exiled.Events.Handlers.Server.WaitingForPlayers += server_handler.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += server_handler.OnRoundStarted;

            Exiled.Events.Handlers.Player.Verified += player_handler.OnVerified;
        }

        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= server_handler.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= server_handler.OnRoundStarted;

            Exiled.Events.Handlers.Player.Verified -= player_handler.OnVerified;

            server_handler = null;
            player_handler = null;
        }
        public override void OnEnabled()
        {
            RegisterEvents();

            if (Config.Debug) { Log.Warn("AnomalousStorms is ready!"); }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();

            if (Config.Debug) { Log.Warn("AnomalousStorms is now disabled!"); }

            base.OnDisabled();
        }

        public void StartStormTimer()
        {
            Timing.RunCoroutine(StormCoroutine());
        }
        private IEnumerator<float> StormCoroutine()
        {
            while (Round.InProgress)
            {
                float delay = (float)(rng.NextDouble() * (420 - 180) + 180);

                if (Config.Debug)
                {
                    delay = 10;
                }

                yield return Timing.WaitForSeconds(delay);

                if (Round.InProgress)
                {
                    TriggerStorm();
                }
            }
        }
        private void TriggerStorm()
        {
            Cassie.Message(
                "anomaly storm detected . please remain calm and proceed with standard protocol.",
                true,
                true,
                true
            );

            foreach (Player player in Player.List)
            {

                if (player.Role.Type.IsScp() || player.Role.Type == RoleTypeId.Spectator || player.Role.Type == RoleTypeId.Tutorial)
                    continue;

                EffectType chosen_effect = PossibleEffects[rng.Next(PossibleEffects.Length)];
                float duration = (float)rng.Next(5, 16);
                Log.Info($"Applying {chosen_effect} to {player.Nickname} for {duration} seconds.");
                player.EnableEffect(chosen_effect, duration, true);
                player.ShowHint($"Your effect: {chosen_effect}. This lasts for {duration} seconds.");
            }
        }
    }
}