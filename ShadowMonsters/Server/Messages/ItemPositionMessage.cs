using ShadowMonsters.Common;

namespace ShadowMonstersServer.Messages
{
    public class ItemPositionMessage
    {
        public ItemPositionMessage(Item source, Vector position)
        {
            Source = source;
            Position = position;
        }
        public Vector Position { get; private set; }
        public Item Source { get; private set; }
    }
}
