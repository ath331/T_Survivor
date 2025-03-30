using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using UnityEngine;

public class WaitingRoomHandler : MonoBehaviour
{
    [SerializeField] private GameObject content;

    public void OnEnable()
    {
        RequestRoom_Strategy.OnRequestRoom += CreateRoomHolder;

        RequestAllRoom_Strategy.OnRequestAllRoom += ShowRoom;

        // ��ü �� ���� ��û
        C_RequestAllRoomInfo requestAllRoomInfo = new C_RequestAllRoomInfo();

        NetworkManager.Instance.Send(requestAllRoomInfo);
    }

    public void OnDisable()
    {
        RequestRoom_Strategy.OnRequestRoom -= CreateRoomHolder;

        RequestAllRoom_Strategy.OnRequestAllRoom -= ShowRoom;
    }

    public void OnClickMakeRoom()
    {
        PopupManager.ShowPopup(nameof(MakeRoomPopup), null, (res) =>
        {
            if (res is C_MakeRoom)
            {
                NetworkManager.Instance.Send(res as C_MakeRoom);
            }
        });
    }

    public void CreateRoomHolder(S_RequestRoomInfo message)
    {
        var roomHolder = ObjectPoolManager.Instance.Get<RoomHolder>("RoomHolder", content.transform);

        roomHolder.SetStatus(message);
    }

    /// <summary>
    /// ���� �� ����� �����ͼ� �����ش�.
    ///   enum ERoomState
    //    {
    //        ROOM_STATE_NONE = 0;
    //        ROOM_STATE_WAITING = 1; // �����
    //        ROOM_STATE_PLAY = 2; // ������
    //        ROOM_STATE_MAX = 3;
    //    }
    //    message RoomInfo
    //    {
    //        int32 num = 1; // ��ȣ
    //        string name = 2; // �̸�
    //        int32 pw = 3; // ��й�ȣ
    //        ERoomState room_state = 4; /// ����
    //        int32 cur_count = 5; // ���� �ο�
    //        int32 max_count = 6; // �ִ� �ο�
    //    }
    /// </summary>
    public void ShowRoom(S_RequestAllRoomInfo roomInfo)
    {
        foreach (var room in roomInfo.RoomList)
        {
            RoomHolder roomHolder = ObjectPoolManager.Instance.Get<RoomHolder>("RoomHolder", content.transform);
            roomHolder.gameObject.SetActive(true);


        }
    }
}
