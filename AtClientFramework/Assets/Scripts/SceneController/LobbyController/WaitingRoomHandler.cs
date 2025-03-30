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

        // 전체 방 갱신 요청
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
    /// 현재 방 목록을 가져와서 보여준다.
    ///   enum ERoomState
    //    {
    //        ROOM_STATE_NONE = 0;
    //        ROOM_STATE_WAITING = 1; // 대기중
    //        ROOM_STATE_PLAY = 2; // 진행중
    //        ROOM_STATE_MAX = 3;
    //    }
    //    message RoomInfo
    //    {
    //        int32 num = 1; // 번호
    //        string name = 2; // 이름
    //        int32 pw = 3; // 비밀번호
    //        ERoomState room_state = 4; /// 상태
    //        int32 cur_count = 5; // 현재 인원
    //        int32 max_count = 6; // 최대 인원
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
