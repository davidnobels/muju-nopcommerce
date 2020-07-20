using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.MyCustomPlugin
{
    internal class MyCustomPluginSettings : ISettings
    {
        public MyCustomPluginSettings()
        {
        }

        public bool UseSandbox { get; set; }
        public string Message { get; set; }
    }
}