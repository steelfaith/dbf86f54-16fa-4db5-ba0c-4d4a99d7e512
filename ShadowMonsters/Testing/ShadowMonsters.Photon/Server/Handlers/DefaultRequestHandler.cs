

namespace ShadowMonsters.Photon.Server.Handlers
{
    public abstract class DefaultRequestHandler : PhotonServerHandler
    {
        protected DefaultRequestHandler(PhotonApplication application) : base(application)
        {
        }
    }
}