using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Network;
using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class LobbyHandler : MonoBehaviour
{
    [SerializeField] private GameObject content;

    [SerializeField] private Button exitButton;

    private List<RoomHolder> roomHolders;

    public void OnEnable()
    {
        PacketEventManager.Subscribe<S_RequestRoomInfo>(CreateRoomHolder);

        PacketEventManager.Subscribe<S_RequestAllRoomInfo>(ShowRoom);

        PacketEventManager.Subscribe<S_DestroyRoom>(DestroyRoom);

        roomHolders = new List<RoomHolder>();

        // 전체 방 갱신 요청
        C_RequestAllRoomInfo requestAllRoomInfo = new C_RequestAllRoomInfo();

        NetworkManager.Instance.Send(requestAllRoomInfo);

        exitButton.onClick.AddListener(OnClickExit);
    }

    public void OnDisable()
    {
        PacketEventManager.Unsubscribe<S_RequestRoomInfo>(CreateRoomHolder);

        PacketEventManager.Unsubscribe<S_RequestAllRoomInfo>(ShowRoom);

        PacketEventManager.Unsubscribe<S_DestroyRoom>(DestroyRoom);

        exitButton.onClick.RemoveAllListeners();

        ReturnRoomHolders();
    }

    private void OnClickExit()
    {

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
        var target = roomHolders.FirstOrDefault(r => r.RoomNumber == message.RoomInfo.Num);

        if (target != null) return;

        var roomHolder = ObjectPoolManager.Instance.Get<RoomHolder>("RoomHolder", content.transform);

        roomHolder.SetStatus(message.RoomInfo);

        roomHolders.Add(roomHolder);
    }

    /// <summary>
    /// 모든 방의 정보를 받는다.  (WaitRoom OnEnable 에서 요청해주고 있음!)
    /// 1. 게임대기실에서 나갔을때 (GameRoom -> WaitRoom)
    /// 2. Splash -> WaitRoom 넘어왔을때 현재 만들어져있는 방들의 정보 가져올때
    /// </summary>
    /// <param name="roomInfo"></param>
    public void ShowRoom(S_RequestAllRoomInfo roomInfo)
    {
        foreach (var room in roomInfo.RoomList)
        {
            var roomHolder = ObjectPoolManager.Instance.Get<RoomHolder>("RoomHolder", content.transform);

            roomHolder.SetStatus(room);

            roomHolders.Add(roomHolder);
        }
    }

    public void DestroyRoom(S_DestroyRoom message)
    {
        // 방 하나만 찾아서 처리
        var target = roomHolders
            .FirstOrDefault(r => r.RoomNumber == message.RoomInfo.Num);

        if (target != null)
        {
            roomHolders.Remove(target);
            Debug.Log($"방 {target.RoomNumber} 반환");
            ObjectPoolManager.Instance.Return(target.gameObject);
        }
        else
        {
            Debug.LogWarning($"삭제할 방 {message.RoomInfo.Num} 을(를) 찾지 못했습니다.");
        }
    }

    /// <summary>
    /// 방 Holder들 제거
    /// </summary>
    public void ReturnRoomHolders()
    {
        foreach (var room in roomHolders)
        {
            ObjectPoolManager.Instance.Return(room.gameObject);
        }
    }
}
