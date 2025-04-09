using System;
using System.Collections;
using System.Collections.Generic;
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
        RequestRoom_Strategy.OnRequestRoom += CreateRoomHolder;

        RequestAllRoom_Strategy.OnRequestAllRoom += ShowRoom;

        roomHolders = new List<RoomHolder>();

        // ��ü �� ���� ��û
        C_RequestAllRoomInfo requestAllRoomInfo = new C_RequestAllRoomInfo();

        NetworkManager.Instance.Send(requestAllRoomInfo);

        exitButton.onClick.AddListener(OnClickExit);
    }

    public void OnDisable()
    {
        RequestRoom_Strategy.OnRequestRoom -= CreateRoomHolder;

        RequestAllRoom_Strategy.OnRequestAllRoom -= ShowRoom;

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
        var roomHolder = ObjectPoolManager.Instance.Get<RoomHolder>("RoomHolder", content.transform);

        roomHolder.SetStatus(message.RoomInfo);

        roomHolders.Add(roomHolder);
    }

    /// <summary>
    /// ��� ���� ������ �޴´�.  (WaitRoom OnEnable ���� ��û���ְ� ����!)
    /// 1. ���Ӵ��ǿ��� �������� (GameRoom -> WaitRoom)
    /// 2. Splash -> WaitRoom �Ѿ������ ���� ��������ִ� ����� ���� �����ö�
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

    /// <summary>
    /// �� Holder�� ����
    /// </summary>
    public void ReturnRoomHolders()
    {
        foreach (var room in roomHolders)
        {
            ObjectPoolManager.Instance.Return(room.gameObject);
        }
    }
}
