using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Network.Handler;
using Protocol;
using UnityEngine;
using UnityEngine.TextCore.Text;


namespace Assets.Scripts.Network.Handler
{
	public partial class PacketHandler
	{
        private Dictionary<ulong, GameObject> _spawnedPlayers = new Dictionary<ulong, GameObject>();

        private void _Process_S_Spawn_Handler( ushort protocolId, byte[] data )
		{
			S_Spawn message = S_Spawn.Parser.ParseFrom( data );

			var playerInfos = message.Players;

			ulong myMecuryId = MercuryHelper.mercuryId;

            foreach (var playerInfo in playerInfos)
            {
                ulong playerId = playerInfo.Id;

                // ��Ʈ��ũ �Ŵ������� �÷��̾� ���� (�ߺ� üũ ����)
                NetworkManager.Instance.ProcessSpawnHandler(playerId);
            }
        }
	}
}