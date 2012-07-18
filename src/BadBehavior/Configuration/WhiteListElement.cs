using System.Configuration;

namespace BadBehavior.Configuration
{
    public class WhiteListElement : ConfigurationElement
    {
        [ConfigurationProperty("ipRanges", IsKey = true, IsDefaultCollection = false)]
        public ValueCollection IPRanges
        {
            get { return (ValueCollection)this["ipRanges"]; }
            set { this["ipRanges"] = value; }
        }

        [ConfigurationProperty("userAgents", IsKey = true, IsDefaultCollection = false)]
        public ValueCollection UserAgents
        {
            get { return (ValueCollection)this["userAgents"]; }
            set { this["userAgents"] = value; }
        }

        [ConfigurationProperty("urls", IsKey = true, IsDefaultCollection = false)]
        public ValueCollection Urls
        {
            get { return (ValueCollection)this["urls"]; }
            set { this["urls"] = value; }
        }
    }
}
