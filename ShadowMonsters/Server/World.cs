using ExitGames.Logging;
using ShadowMonsters.Common;
using System.Collections.Concurrent;

namespace ShadowMonstersServer
{
    public class World
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public World(string name, BoundingBox boundingBox, Vector tileDimensions)
        {
            Name = name;
            ItemCache = new ConcurrentDictionary<string, Item>();
            _logger.InfoFormat("created world {0}", name);
        }

        public string Name { get; private set; }
        public ConcurrentDictionary<string,Item> ItemCache { get; private set; }
    }
}
