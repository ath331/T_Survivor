using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using Assets.Scripts.Network;

public class TitleController : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputIp;
    [SerializeField] private TMP_InputField inputPort;

    public void OnConnectedToServer()
    {
        NetworkManager.Instance.ConnectToTcpServer(inputIp.text, inputPort.text);
    }
}
