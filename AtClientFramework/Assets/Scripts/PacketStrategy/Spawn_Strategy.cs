using Protocol;
using UnityEngine;

public class Spawn_Strategy : IStrategy
{
    public Spawn_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_Spawn>(OnSpawnPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_Spawn>(OnSpawnPacketReceived);
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
