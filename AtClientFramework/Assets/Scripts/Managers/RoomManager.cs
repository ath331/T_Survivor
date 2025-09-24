using System;
using Assets.Scripts.Network;
using Protocol;
using UnityEngine;

// 현재 방의 '상태(모델)'를 소유하고 네트워크 이벤트를 처리
public class RoomManager : SingletonMonoBehaviour<RoomManager>
{
    // RoomModel을 갖고있음.
    public RoomModel roomModel { get; private set; }

    public event Action<LobbyStatus> OnChangeStatus;

    public bool isInitialized = false;

    public ObjectInfo myPlayerInfo;

    public override void Initialize()
    {
        if (isInitialized) 
            return;

        isInitialized = true;

        roomModel = new RoomModel();
    }

    private void OnEnable()
    {
        PacketEventManager.Subscribe<S_MakeRoom>(OnReceiveMakeRoom);
        PacketEventManager.Subscribe<S_WaitingRoomEnter>(OnReceiveEnterRoom);
        PacketEventManager.Subscribe<S_WaitingRoomEnterNotify>(OnReceivePlayerEnterNotify);
        PacketEventManager.Subscribe<S_WaitingRoomOutNotify>(OnReceivePlayerOutNotify);
        PacketEventManager.Subscribe<S_ChangeWaitingState>(OnReceiveChangeState);
        PacketEventManager.Subscribe<S_ChangeWaitingStateNotify>(OnReceiveChangeStateNotify);
    }

    private void OnDisable()
    {
        PacketEventManager.Unsubscribe<S_MakeRoom>(OnReceiveMakeRoom);
        PacketEventManager.Unsubscribe<S_WaitingRoomEnter>(OnReceiveEnterRoom);
        PacketEventManager.Unsubscribe<S_WaitingRoomEnterNotify>(OnReceivePlayerEnterNotify);
        PacketEventManager.Unsubscribe<S_WaitingRoomOutNotify>(OnReceivePlayerOutNotify);
        PacketEventManager.Unsubscribe<S_ChangeWaitingState>(OnReceiveChangeState);
        PacketEventManager.Unsubscribe<S_ChangeWaitingStateNotify>(OnReceiveChangeStateNotify);
    }

    public void SetMyPlayerInfo(ObjectInfo plyaerInfo)
    {
        myPlayerInfo = plyaerInfo;
    }

    // --- 패킷 핸들러들 ---

    /// <summary>
    /// 내가 방을 만든다.
    /// </summary>
    /// <param name="message"></param>
    private void OnReceiveMakeRoom(S_MakeRoom message)
    {
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            roomModel.Clear();

            roomModel.UpdateRoomInfo(message.MadeRoomInfo);

            roomModel.AddPlayer(myPlayerInfo, 0, true);

            OnChangeStatus?.Invoke(LobbyStatus.WaitRoom);
        }
    }

    /// <summary>
    /// 내가 방에 들어간다.
    /// </summary>
    private void OnReceiveEnterRoom(S_WaitingRoomEnter message)
    {
        if (message.Result == EResultCode.ResultCodeSuccess)
        {
            roomModel.Clear();

            roomModel.UpdateRoomInfo(message.RoomInfo);

            OnChangeStatus?.Invoke(LobbyStatus.WaitRoom);
        }
    }

    /// <summary>
    /// 대기실에 있는 애들이 받는거, 내가 들어가서 이것도 받음.
    /// </summary>
    private void OnReceivePlayerEnterNotify(S_WaitingRoomEnterNotify message)
    {
        roomModel.AddPlayer(message.Player, message.EnterCount, false);
    }

    private void OnReceivePlayerOutNotify(S_WaitingRoomOutNotify message)
    {
        roomModel.RemovePlayer(message.Player.Id);
    }

    private void OnReceiveChangeState(S_ChangeWaitingState message)
    {
        bool isReady = (message.State == EWaitingState.WaitingStateRaedy);

        roomModel.SetPlayerReadyState(myPlayerInfo.Id, isReady);
    }

    private void OnReceiveChangeStateNotify(S_ChangeWaitingStateNotify message)
    {
        bool isReady = (message.State == EWaitingState.WaitingStateRaedy);

        roomModel.SetPlayerReadyState(message.Player.Id, isReady);
    }
}