using Protocol;
using UnityEngine;

public class Chat_Strategy
{
    public Chat_Strategy()
    {
        PacketEventManager.Subscribe<S_Chat>(OnChatPacketReceived);
    }

    private void OnChatPacketReceived(S_Chat message)
    {
        Debug.Log(message.Msg);
    }
}
