using Common;
using Server.Common.Interfaces;

namespace Server.Common
{
    public class Character
    {
        public int Id { get; }
        public int AccountId { get; set; }
        public int UserId { get; }
        public string Name { get; set; }
        public IWorldRegionInstance WorldRegionInstance { get; set; }
        public Vector3 CurrentPosition { get; set; }
        public Vector3? NextPosition { get; set; }
        public Vector3 Forward { get; set; }

        public Character(int characterId, int userId)
        {
            Id = characterId;
            UserId = userId;
        }
    }
}