using System.Configuration;

namespace BieberBlocker.Configuration
{
    internal class BlockedArtistsSection : ConfigurationSection
    {

        [ConfigurationProperty("Artists", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ArtistsCollection),
            AddItemName = "artist",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ArtistsCollection Artists
        {
            get
            {
                return base["Artists"] as ArtistsCollection;
            }
        }
    }
}
