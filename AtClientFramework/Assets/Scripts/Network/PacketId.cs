using System;
using UnityEngine;


namespace Assets.Scripts.Network
{
    enum EPacketId
    {
        PKT_C_Login = 1000,
        PKT_S_Login = 1001,
        PKT_C_EnterLobby = 1002,
        PKT_S_EnterLobby = 1003,
        PKT_C_WaitingRoomEnter = 1004,
        PKT_S_WaitingRoomEnter = 1005,
        PKT_S_WaitingRoomEnterNotify = 1006,
        PKT_C_MakeRoom = 1007,
        PKT_S_MakeRoom = 1008,
        PKT_S_DestroyRoom = 1009,
        PKT_S_RequestRoomInfo = 1010,
        PKT_C_RequestAllRoomInfo = 1011,
        PKT_S_RequestAllRoomInfo = 1012,
        PKT_C_EnterGame = 1013,
        PKT_S_EnterGame = 1014,
        PKT_C_EnterGameFinish = 1015,
        PKT_S_EnterGameFinish = 1016,
        PKT_C_LeaveGame = 1017,
        PKT_S_LeaveGame = 1018,
        PKT_C_Move = 1019,
        PKT_S_Move = 1020,
        PKT_S_Spawn = 1021,
        PKT_S_DeSpawn = 1022,
        PKT_C_Chat = 1023,
        PKT_S_Chat = 1024,
        PKT_C_AnimationEvent = 1025,
        PKT_S_AnimationEvent = 1026,
    }

    public class PacketIdFactory
    {
        public static ushort GetPacketId( string packetName )
        {
            try
            {
                packetName = "PKT_" + packetName;

                if ( Enum.TryParse< EPacketId >( packetName, true, out var result ) )
                    return (ushort)( result );

                // Debug.LogError( $"Invalid packet name: {packetName}" );
                return 0;
            }
            catch ( Exception ex )
            {
                Debug.LogError( $"Error parsing packet name: {packetName}. Exception: {ex}" );
                return 0;
            }
        }
    }
}