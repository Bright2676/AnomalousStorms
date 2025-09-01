//authors: bright2676

using Exiled.API.Features;
using static AnomalousStorms.Plugin;

namespace AnomalousStorms.Events
{
    internal sealed class ServerHandler
    {
        public void OnWaitingForPlayers()
        {
            Log.Info("AnomalousStorms is now waiting for players...");
        }

        public void OnRoundStarted()
        {
            Log.Info("A new round has started.");
            Plugin.Instance.StartStormTimer();
        }
    }
}
