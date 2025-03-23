using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network;
using Protocol;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakeRoomPopup : BasePopupHandler
{
    [SerializeField] TMP_InputField titleText;
    [SerializeField] TMP_InputField passwordText;

    private void OnEnable()
    {
        
    }

    /// <param name="param"></param>
    public override void OnBeforeEnter(object param)
    {
        base.OnBeforeEnter(param);
    }

    public void OnClickMakeRoom()
    {
        string inputText = titleText.text.Trim(); // ���� ����

        // �ؽ�Ʈ�� ����ְų� ���̰� 2���� ���� return
        if (string.IsNullOrEmpty(inputText) || inputText.Length <= 2)
        {
            Debug.Log("�Է°��� ����ְų� 2���� �����Դϴ�.");
            return;
        }

        int pw = -1;
        string pwInput = passwordText.text.Trim();

        if (!string.IsNullOrEmpty(pwInput))
        {
            // ���ڸ� �Էµǵ��� ���ѵǾ� �ִ���, Ȥ�� �𸣴� �Ľ� �õ�
            if (!int.TryParse(pwInput, out pw))
            {
                Debug.Log("�н������ ���ڸ� �Է� �����մϴ�.");
                return;
            }
        }

        C_MakeRoom makeRoomPkt = new C_MakeRoom()
        {
            RoomInfo = new RoomInfo
            {
                Name = titleText.text,

                Pw = pw,

                CurCount = 1,

                MaxCount = 3,

                RoomState = ERoomState.RoomStateWaiting,
            }
        };

        NetworkManager.Instance.Send(makeRoomPkt);

        PopupManager.ClosePopup(makeRoomPkt);
    }
}
