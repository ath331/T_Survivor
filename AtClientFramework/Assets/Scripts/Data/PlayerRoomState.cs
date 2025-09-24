// PlayerStateInfo.cs (수정됨)

using Protocol;

public class PlayerStateInfo
{
    public ObjectInfo Info { get; private set; }
    public ulong JoinSequence { get; private set; } // enterCount 값을 저장
    public bool IsReady { get; set; }
    public bool IsLeader { get; set; }

    // 생성자가 enterCount를 별도로 받도록 수정
    public PlayerStateInfo(ObjectInfo info, ulong enterCount, bool isLeader = false)
    {
        this.Info = info;
        this.JoinSequence = enterCount; // 전달받은 enterCount 값으로 설정
        this.IsReady = false;
        this.IsLeader = isLeader;
    }
}