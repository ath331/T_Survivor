using Protocol;
using UnityEngine;

public class Animation_Strategy : IStrategy
{
    public Animation_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_AnimationEvent>(OnAnimationPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_AnimationEvent>(OnAnimationPacketReceived);
    }

    private void OnAnimationPacketReceived(S_AnimationEvent message)
    {
        var playerId = message.PlayerId;
        string animationType = message.AnimationType;
        EAnimationParamType paramType = message.ParamType;
        bool boolVal = message.BoolValue;

        if (PlayerListManager.Instance.TryGetPlayer(playerId, out PlayerController player))
        {
            player.PlayNetworkAnimation(animationType, paramType, boolVal);
        }
    }
}
