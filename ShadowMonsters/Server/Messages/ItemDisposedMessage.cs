
namespace ShadowMonstersServer.Messages
{
    public class ItemDisposedMessage
    {
        public ItemDisposedMessage(Item source)
        {
            Source = source;
        }
        public Item Source { get; private set; }
    }
}
