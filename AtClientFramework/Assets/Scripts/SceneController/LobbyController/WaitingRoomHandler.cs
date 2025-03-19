using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using UnityEngine;

public class WaitingRoomHandler : MonoBehaviour
{
    public static event Action wait;

    [SerializeField] private GameObject content;

    public void OnEnable()
    {
        // 전체 방 갱신 요청
        C_RequestAllRoomInfo requestAllRoomInfo = new C_RequestAllRoomInfo();

        NetworkManager.Instance.Send(requestAllRoomInfo);
    }

    /// <summary>
    /// 현재 방 목록을 가져와서 보여준다.
    /// </summary>
    public void ShowRoom()
    {
        GameObject roomHolder = ObjectPoolManager.Instance.Get("RoomHolder");
        roomHolder.transform.parent = content.transform;
        roomHolder.SetActive(true);
    }
}
