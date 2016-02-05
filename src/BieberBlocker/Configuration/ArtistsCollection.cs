using System.Configuration;

namespace BieberBlocker.Configuration
{
    internal class ArtistsCollection : ConfigurationElementCollection
    {

        public ArtistElement this[int index]
        {
            get
            {
                return BaseGet(index) as ArtistElement;
            }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public void Add(ArtistElement item)
        {
            BaseAdd(item);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ArtistElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var artist = element as ArtistElement;
            return artist == null ? null : artist.Name;
        }
    }
}