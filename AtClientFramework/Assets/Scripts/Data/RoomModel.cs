using System;
using System.Collections.Generic;
using System.Linq;
using Protocol;
using UnityEngine.Playables;

// 현재 접속해 있는 방의 모든 '데이터'를 담는 클래스
public class RoomModel
{
    public int RoomNumber { get; private set; }
    public string RoomName { get; private set; }
    public int CurrentPlayers => Players.Count;
    public int MaxPlayers { get; private set; }

    // 플레이어 ID를 Key로 사용하여 플레이어 정보에 접근
    public Dictionary<ulong, PlayerStateInfo> Players { get; private set; } = new Dictionary<ulong, PlayerStateInfo>();

    // 이 모델의 데이터가 변경될 때마다 호출될 이벤트
    public event Action OnModelUpdated;

    // RoomInfo의 기본 정보만 업데이트하는 메서드
    public void UpdateRoomInfo(RoomInfo roomInfo)
    {
        RoomNumber = roomInfo.Num;
        RoomName = roomInfo.Name;
        MaxPlayers = roomInfo.MaxCount;

        NotifyModelUpdate();
    }

    // 방을 나갈 때 또는 새로운 방 정보를 받을 때 모든 데이터를 초기화하는 메서드
    public void Clear()
    {
        Players.Clear();
        RoomNumber = 0;
        RoomName = "";
        NotifyModelUpdate();
    }

    // 특정 플레이어의 레디 상태 변경
    public void SetPlayerReadyState(ulong playerId, bool isReady)
    {
        if (Players.TryGetValue(playerId, out PlayerStateInfo player))
        {
            player.IsReady = isReady;

            NotifyModelUpdate();
        }
    }

    // 플레이어 입장
    public void AddPlayer(ObjectInfo playerInfo, ulong enterCount, bool isLeader)
    {
        if (!Players.ContainsKey(playerInfo.Id))
        {
            // PlayerStateInfo 생성자에 enterCount를 넘겨줌
            Players[playerInfo.Id] = new PlayerStateInfo(playerInfo, enterCount, isLeader);
            NotifyModelUpdate();
        }
    }

    // 플레이어 퇴장
    public void RemovePlayer(ulong playerId)
    {
        if (Players.Remove(playerId))
        {
            NotifyModelUpdate();
        }
    }

    // 데이터 변경을 외부에 알리는 메서드
    private void NotifyModelUpdate()
    {
        OnModelUpdated?.Invoke();
    }
}