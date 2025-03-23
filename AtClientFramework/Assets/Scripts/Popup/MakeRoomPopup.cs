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
        string inputText = titleText.text.Trim(); // 공백 제거

        // 텍스트가 비어있거나 길이가 2글자 이하 return
        if (string.IsNullOrEmpty(inputText) || inputText.Length <= 2)
        {
            Debug.Log("입력값이 비어있거나 2글자 이하입니다.");
            return;
        }

        int pw = -1;
        string pwInput = passwordText.text.Trim();

        if (!string.IsNullOrEmpty(pwInput))
        {
            // 숫자만 입력되도록 제한되어 있더라도, 혹시 모르니 파싱 시도
            if (!int.TryParse(pwInput, out pw))
            {
                Debug.Log("패스워드는 숫자만 입력 가능합니다.");
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
