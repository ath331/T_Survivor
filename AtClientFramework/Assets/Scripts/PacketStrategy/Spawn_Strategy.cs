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
            // �Ŵ������� �÷��̾� ���� (�ߺ� üũ ����)
            PlayerListManager.Instance.ProcessSpawnHandler(playerInfo);
        }
    }
}
