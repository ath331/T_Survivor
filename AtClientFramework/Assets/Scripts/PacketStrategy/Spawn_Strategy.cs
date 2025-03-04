using Protocol;
using UnityEngine;

public class Spawn_Strategy
{
    public Spawn_Strategy()
    {
        PacketEventManager.Subscribe<S_Spawn>(OnSpawnPacketReceived);
    }

    private void OnSpawnPacketReceived(S_Spawn message)
    {
        var playerInfos = message.Players;

        foreach (var playerInfo in playerInfos)
        {
            // 매니저에서 플레이어 생성 (중복 체크 포함)
            PlayerListManager.Instance.ProcessSpawnHandler(playerInfo);
        }
    }
}
