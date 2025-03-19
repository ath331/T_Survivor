using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using UnityEngine;

public class MakeRoomPopup : BasePopupHandler
{
    public override void OnClickClose()
    {
        // TODO : ���� PopupManager.Close �� �ٲ� ��
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
