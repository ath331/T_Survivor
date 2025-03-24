using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using UnityEngine;

public class WaitingRoomHandler : MonoBehaviour
{
    public static Action<S_RequestAllRoomInfo> OnRequestAllRoomInfo;

    [SerializeField] private GameObject content;

    public void OnEnable()
    {
        OnRequestAllRoomInfo += ShowRoom;

        // ��ü �� ���� ��û
        C_RequestAllRoomInfo requestAllRoomInfo = new C_RequestAllRoomInfo();

        NetworkManager.Instance.Send(requestAllRoomInfo);
    }

    public void OnDisable()
    {
        OnRequestAllRoomInfo -= ShowRoom;
    }

    public void CreateRoomHolder(S_MakeRoom message)
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
