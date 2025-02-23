using UnityEngine;

public interface IPlayerState
{
    /// <summary>
    /// ���� ���� �� ȣ�� (�ʱ�ȭ, �ִϸ��̼� ��ȯ ��)
    /// </summary>
    void Enter(PlayerController player);

    /// <summary>
    /// ���� ���� �� ȣ�� (���� ���� ��)
    /// </summary>
    void Exit();

    /// <summary>
    /// �� ������ �Է��� ó��
    /// </summary>
    void HandleInput();

    /// <summary>
    /// �� ������ ���� ������Ʈ (�ִϸ��̼�, Ÿ�̸� ��)
    /// </summary>
    void UpdateState();

    /// <summary>
    /// ���� ������Ʈ ���� ���� (�̵� ��)
    /// </summary>
    void FixedUpdateState();
}
