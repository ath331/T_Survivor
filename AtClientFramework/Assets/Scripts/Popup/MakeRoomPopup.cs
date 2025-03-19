using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using UnityEngine;

public class MakeRoomPopup : BasePopupHandler
{
    public override void OnClickClose()
    {
        // TODO : 추후 PopupManager.Close 로 바꿀 것
        Destroy(this.gameObject);
    }

    public void OnClickMakeRoom()
    {
        C_MakeRoom makeRoomPkt = new C_MakeRoom()
        {
            RoomInfo = new RoomInfo
            {

            }
        };

        NetworkManager.Instance.Send(makeRoomPkt);
    }
}
