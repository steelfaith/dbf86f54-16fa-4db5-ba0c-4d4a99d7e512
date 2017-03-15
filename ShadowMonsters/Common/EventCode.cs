namespace ShadowMonsters.Common
{
    public enum EventCode : byte
    {
        ItemDestroyed = 1,
        ItemMoved,
        ItemPropertiesSet,
        WorldExited,
        ItemSubscribed,
        ItemUnsubscribed,
        ItemProperties,
        CounterData,
    }
}