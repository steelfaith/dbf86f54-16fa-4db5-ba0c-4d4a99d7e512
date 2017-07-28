using Common;
using Common.Messages;

namespace Server.Common.Interfaces
{
    public interface IBattleInstance : IServerInstance
    {
        void AttemptRun(InstanceMessage instanceMessage);
    }
}