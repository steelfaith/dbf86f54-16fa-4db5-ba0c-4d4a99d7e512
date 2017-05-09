using Common;
using Server.Common.Interfaces;

namespace Server.Common
{
    public class Character
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public IWorldRegionInstance WorldRegionInstance { get; set; }
        public Vector3 CurrentPosition { get; set; }
        public Vector3? NextPosition { get; set; }
    }
}