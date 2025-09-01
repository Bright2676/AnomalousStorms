using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace AnomalousStorms
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public bool EnableJoinMessage { get; set; } = false;

    }
}