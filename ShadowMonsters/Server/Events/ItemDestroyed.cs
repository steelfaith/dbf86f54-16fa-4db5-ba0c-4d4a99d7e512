using Photon.SocketServer.Rpc;
using ShadowMonsters.Common;

namespace ShadowMonstersServer.Events
{
    public class ItemDestroyed
    {
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }
    }
}