using System;

namespace ShadowMonsters.Framework
{
    public interface IBackgroundThread
    {
        void Setup();
        void Run(Object threadContext);
        void Stop();
    }
}