﻿using System;
using Common.Interfaces;
using Common.Messages;

namespace Assets.ServerStubHome
{
    public class BattleInstanceMessageHandler : IMessageHandler
    {
        public OperationCode OperationCode
        {
            get
            {
                return OperationCode.CreateBattleInstanceResponse;
            }
        }

        public void HandleMessage(RouteableMessage routeableMessage)
        {
            throw new NotImplementedException();
        }
    }
}
