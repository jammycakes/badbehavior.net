using System.Configuration;

namespace BadBehavior.Configuration
{
    [ConfigurationCollection(typeof(ValueElement))]
    public class ValueCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ValueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ValueElement)element).Value;
        }

        public ValueElement this[int index]
        {
            get { return (ValueElement)BaseGet(index); }
        }

        new public ValueElement this[string key]
        {
            get { return (ValueElement)BaseGet(key); }
        }
    }
}
