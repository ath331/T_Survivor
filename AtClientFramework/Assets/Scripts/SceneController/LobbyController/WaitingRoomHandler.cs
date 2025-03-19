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
        // ��ü �� ���� ��û
        C_RequestAllRoomInfo requestAllRoomInfo = new C_RequestAllRoomInfo();

        NetworkManager.Instance.Send(requestAllRoomInfo);
    }

    /// <summary>
    /// ���� �� ����� �����ͼ� �����ش�.
    /// </summary>
    public void ShowRoom()
    {
        GameObject roomHolder = ObjectPoolManager.Instance.Get("RoomHolder");
        roomHolder.transform.parent = content.transform;
        roomHolder.SetActive(true);
    }
}
