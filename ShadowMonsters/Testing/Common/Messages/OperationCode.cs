namespace Common.Messages
{
    public enum OperationCode
    {
        ConnectRequest = 0,
        ConnectResponse = 1,
        CreateBattleInstanceRequest = 3,
        CreateBattleInstanceResponse = 4,
        BattleInstanceRunRequest = 5,
        BattleInstanceRunResponse = 6,
        PlayerMoveRequest = 7,
        SelectCharacterRequest = 8,
        SelectCharacterResponse = 9,
        PlayerMoveEvent = 10,
        ShadowCreatedEvent = 11,
        PlayerConnectedEvent = 12,
    }
}