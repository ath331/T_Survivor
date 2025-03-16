using Assets.Scripts.Network;
using Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatController : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private TMP_InputField inputField;

    private const int LimitChatPrefab = 20;
    private const int LimitChatLength = 50;

    // �ؾ� �� �� inputfield�� �Է��ؼ� ����/�Է� ��ư�� ���� �� ä���� �����ϰ� content�ȿ� �ڽ� �������� �����ؼ� �־�� �Ѵ�!

    private void Update()
    {
        if (inputField.isFocused && Input.GetKeyDown(KeyCode.Return))   // ä��â�� ������ ����Ű�� ���� ��
        {
            Send_Message();
        }
    }

    public void Send_Message()
    {
        string msg = inputField.text.Trim();

        // ����ִ� �޽����� ������ ����.
        if (string.IsNullOrEmpty(msg)) return;

        GameObject chatText = ObjectPoolManager.Instance.Get("ChatText");
        chatText.transform.parent = content.transform;
        chatText.SetActive(true);

        C_Chat pkt = new C_Chat();

        pkt.Msg = msg;

        inputField.text = string.Empty;
        inputField.ActivateInputField();

        NetworkManager.Instance.Send(pkt);
    }
}
