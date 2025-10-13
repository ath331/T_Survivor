using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Protocol;
using Assets.Scripts.Network;
using System;

public class ServerCheatManager : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private GameObject cheatPanel;
    [SerializeField] private TMP_InputField inputField;

    [Header("입력 설정")]
    [SerializeField] private float requiredHoldTime = 1f;

    // 다른 스크립트에서 접근할 수 있는 공용 입력 잠금 플래그
    public static bool IsInputBlocked { get; private set; } = false;

    private float holdTimer = 0f;
    private bool cheatToggleProcessed = false;

    void Start()
    {
        cheatPanel.SetActive(false);
    }

    void Update()
    {
#if UNITY_EDITOR
        // 마우스 왼쪽(0)과 오른쪽(1) 버튼이 '동시에' 눌려있는지 확인
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= requiredHoldTime && !cheatToggleProcessed)
            {
                SetPanelActive(!cheatPanel.activeSelf);
                cheatToggleProcessed = true;
            }
        }
        else
        {
            // 버튼 중 하나라도 떼면 타이머와 플래그를 리셋
            holdTimer = 0f;
            cheatToggleProcessed = false;
        }

        // 치트 패널이 활성화되어 있고, Enter 키를 눌렀을 때
        if (cheatPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            OnSendCheat();
        }
#endif
    }

    private void OnSendCheat()
    {
        try
        {
            string message = inputField.text;
            if (string.IsNullOrEmpty(message)) return;

            string cheatMessage = "@" + message;

            C_Chat pkt = new C_Chat { Msg = cheatMessage };
            NetworkManager.Instance.Send(pkt);

            ToastMessage.Show("치트 전송", transform);

            inputField.text = string.Empty;
            inputField.ActivateInputField();
        }
        catch (Exception ex)
        {
            Debug.LogError($"데이터 수신 중 오류 발생: {ex.Message}");

            inputField.text = string.Empty;
            inputField.ActivateInputField();
        }
    }

    /// <summary>
    /// 치트 패널의 활성 상태를 설정하고, 전역 입력 잠금 플래그를 업데이트합니다.
    /// </summary>
    private void SetPanelActive(bool isActive)
    {
        cheatPanel.SetActive(isActive);
        IsInputBlocked = isActive;

        if (isActive)
        {
            inputField.text = string.Empty;
            inputField.ActivateInputField();
        }
    }
}