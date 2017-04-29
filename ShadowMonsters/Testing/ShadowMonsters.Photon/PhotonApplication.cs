namespace ShadowMonsters.Photon
{
    public abstract class PhotonApplication
    {
        public abstract byte SubCodeParameterKey { get; }
        public PhotonConnectionCollection ConnectionCollection { get; private set; }
    }
}