using Protocol;
using UnityEngine;

public class Login_Strategy : IStrategy
{
    public Login_Strategy()
    {
        Register();
    }

    public void Register()
    {
        PacketEventManager.Subscribe<S_Login>(OnLoginPacketReceived);
    }

    public void Unregister()
    {
        PacketEventManager.Unsubscribe<S_Login>(OnLoginPacketReceived);
    }

    private void OnLoginPacketReceived(S_Login message)
    {
        Debug.Log("·Î±×ÀÎ");
    }
}
