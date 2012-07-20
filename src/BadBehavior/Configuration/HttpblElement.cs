using System.Configuration;

namespace BadBehavior.Configuration
{
    public class HttpblElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("threatLevel", IsRequired = false, DefaultValue = 25)]
        public int ThreatLevel
        {
            get { return (int)this["threatLevel"]; }
            set { this["threatLevel"] = value; }
        }

        [ConfigurationProperty("maxAge", IsRequired = false, DefaultValue = 30)]
        public int MaxAge
        {
            get { return (int)this["maxAge"]; }
            set { this["maxAge"] = value; }
        }
    }
}
