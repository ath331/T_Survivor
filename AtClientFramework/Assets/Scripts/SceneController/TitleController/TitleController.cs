using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using Assets.Scripts.Network;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

public class TitleController : MonoBehaviour
{
    public Toggle toggleLocal;

    bool isCheckLocal = true;

    readonly string local_Ip = "127.0.0.1";
    readonly string local_Port = "7777";

    void Start()
    {
        toggleLocal.onValueChanged.AddListener(OnToggleLocalValueChanged);
    }

    public void OnConnectedToServer()
    {
        if (isCheckLocal)
        {
            NetworkManager.Instance.ConnectToTcpServer(local_Ip, local_Port);

            return;
        }

        NetworkManager.Instance.ConnectToTcpServer("", "");
    }

    void OnToggleLocalValueChanged(bool val)
    {
        isCheckLocal = val;
    }
}
