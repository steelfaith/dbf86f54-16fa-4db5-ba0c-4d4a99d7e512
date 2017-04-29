using Assets.Infrastructure.UI.Controllers;

namespace Assets.Infrastructure.UI.Views
{
    public interface IView
    {
        IViewController Controller { get; }
    }
}