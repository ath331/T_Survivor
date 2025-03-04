using Protocol;
using UnityEngine;
public class Login_Strategy
{
    public Login_Strategy()
    {
        PacketEventManager.Subscribe<S_Login>(OnLoginPacketReceived);
    }

    private void OnLoginPacketReceived(S_Login message)
    {
        Debug.Log("·Î±×ÀÎ");
    }
}
