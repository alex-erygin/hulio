using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace huliobot
{
    public static class SettingsStore
    {
        public static readonly Dictionary<string, string> Tokens = new Dictionary<string, string>();

        static SettingsStore()
        {
            var settings = XDocument.Parse(File.ReadAllText(@"SecretSettings.xml"));
            foreach (var setting in settings.Root.Elements())
            {
                Tokens[setting.Attribute("key").Value] = setting.Attribute("value").Value;
            }
        }
        
    }
}