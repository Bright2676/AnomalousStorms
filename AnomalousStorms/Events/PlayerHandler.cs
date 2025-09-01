//authors: bright2676

using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using static AnomalousStorms.Plugin;

namespace AnomalousStorms.Events
{
    internal sealed class PlayerHandler
    {
        public void OnVerified(VerifiedEventArgs ev)
        {
            if (ev.Player == null) { return; }

            var player = ev.Player;
            
            if (Instance.Config.EnableJoinMessage) { player.ShowHint("Welcome! This server is using AnomalousStorms, enjoy your stay!"); }

        }
    }
}