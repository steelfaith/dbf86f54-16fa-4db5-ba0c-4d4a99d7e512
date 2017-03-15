using Photon.SocketServer.Rpc;
using ShadowMonsters.Common;

namespace ShadowMonstersServer.Events
{
    public class ItemMoved
    {
        [DataMember(Code = (byte)ParameterCode.ItemId)]
        public string ItemId { get; set; }

        [DataMember(Code = (byte)ParameterCode.OldPosition)]
        public Vector OldPosition { get; set; }

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position { get; set; }

        [DataMember(Code = (byte)ParameterCode.Rotation, IsOptional = true)]
        public Vector Rotation { get; set; }

        [DataMember(Code = (byte)ParameterCode.OldRotation, IsOptional = true)]
        public Vector OldRotation { get; set; }
    }
}