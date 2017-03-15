using ShadowMonsters.Common;

namespace ShadowMonstersServer
{
    public class ItemSnapshot
    {
        public ItemSnapshot(Item source, Vector position, Vector rotation, int propertiesRevision)
        {
            Source = source;
            Position = position;
            Rotation = rotation;
            PropertiesRevision = propertiesRevision;
        }

        public Item Source { get; private set; }
        public Vector Position { get; private set; }
        public Vector Rotation { get; private set; }
        public int PropertiesRevision { get; private set; }
    }
}