using UnityEngine;

public interface IPlayerState
{
    /// <summary>
    /// 상태 진입 시 호출 (초기화, 애니메이션 전환 등)
    /// </summary>
    void Enter(PlayerController player);

    /// <summary>
    /// 상태 종료 시 호출 (상태 정리 등)
    /// </summary>
    void Exit();

    /// <summary>
    /// 매 프레임 입력을 처리
    /// </summary>
    void HandleInput();

    /// <summary>
    /// 매 프레임 로직 업데이트 (애니메이션, 타이머 등)
    /// </summary>
    void UpdateState();

    /// <summary>
    /// 물리 업데이트 관련 로직 (이동 등)
    /// </summary>
    void FixedUpdateState();
}
