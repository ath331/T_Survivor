using System;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;
using System.Threading.Tasks;
using Assets.Scripts.Network.Handler;
using Unity.VisualScripting;
using Assets.Scripts.Network.Packet.Room;
using Cysharp.Threading.Tasks;
using Assets.Scripts.Network.Packet.Game;
using System.Collections.Generic;


namespace Assets.Scripts.Network
{
    public class NetworkManager
    {
        private static readonly NetworkManager _instance = new NetworkManager();
        public static NetworkManager Instance => _instance;
        private Dictionary<ulong, GameObject> _spawnedPlayers = new Dictionary<ulong, GameObject>();

        private TcpClient _socketConnection;
        private NetworkStream _stream;
        private bool _isConnected = false;
        private bool _isInitialized = false;

        private Packet_C_EnterLobby enterLobbyPacket;
        private Packet_C_EnterGame enterEnterGamePacket;
        private Packet_C_EnterGameFinish enterEnterGameFinishPacket;

        /// <summary>
        /// ��Ʈ��ũ �Ŵ��� �ʱ�ȭ �޼���.
        /// �ܺο��� �� �� ȣ���Ͽ� �ʿ��� �ʱ�ȭ�� �����մϴ�.
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized)
                return;

            // �ʿ��� �ʱ�ȭ �۾� ���� (ex: ��Ŷ �ν��Ͻ� ����)
            enterLobbyPacket = new Packet_C_EnterLobby();
            enterEnterGamePacket = new Packet_C_EnterGame();
            enterEnterGameFinishPacket = new Packet_C_EnterGameFinish();

            _spawnedPlayers = new Dictionary<ulong, GameObject>();

            _isInitialized = true;

            Debug.Log("Network Manager Initialized.");
        }

        /// <summary> �÷��̾� ���� �ڵ鷯 (�ߺ� üũ) </summary>
        public void ProcessSpawnHandler(ulong playerId)
        {
            // �̹� ������ �÷��̾��� ����
            if (_spawnedPlayers.ContainsKey(playerId))
                return;

            // ���� ��ġ ����
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(1f, 20f), 0f, UnityEngine.Random.Range(1f, 20f));

            // ������Ʈ Ǯ���� �÷��̾� ��������
            GameObject playerObject = ObjectPoolManager.Instance.Get("Character");
            playerObject.transform.position = spawnPosition;
            playerObject.SetActive(true);

            // �� ĳ�������� Ȯ���ϰ� IsLocalPlayer Ȱ��ȭ/��Ȱ��ȭ
            PlayerController controller = playerObject.GetComponent<PlayerController>();
            controller.IsLocalPlayer = (playerId == MercuryHelper.mercuryId);

            // ������ �÷��̾� ����
            _spawnedPlayers[playerId] = playerObject;
            Debug.Log($"[NetworkManager] �÷��̾� {playerId} ������.");
        }

        /// <summary> �÷��̾� ���� (������ ��) </summary>
        public void RemovePlayer(ulong playerId)
        {
            if (_spawnedPlayers.ContainsKey(playerId))
            {
                GameObject playerObject = _spawnedPlayers[playerId];
                ObjectPoolManager.Instance.Return("Character", playerObject); // ������Ʈ Ǯ�� ��ȯ
                _spawnedPlayers.Remove(playerId);
                Debug.Log($"[NetworkManager] �÷��̾� {playerId} ���ŵ�.");
            }
        }

        public async void ConnectToTcpServer(string inputIp, string inputPort)
        {
            // if ( mode.options[ mode.value ].text == "Single" )
            // {
            //     Debug.Log( "SingleMode Start" );
            //     return;
            // }

            if (_isConnected)
            {
                Debug.Log("Already Connected");
                return;
            }

            try
            {
                // string ip = inputIp.text;
                // int port = 0;
                // if ( int.TryParse( inputPort.text, out int intValue ) )
                //     port = intValue;

                string ip = inputIp;
                int port = int.Parse(inputPort);

                _socketConnection = new TcpClient(ip, port);
                _stream = _socketConnection.GetStream();
                _isConnected = true;
            }
            catch (Exception e)
            {
                Debug.LogError("On client connect exception " + e);
            }

            if (_isConnected)
            {
                Debug.Log("���� ���� ����, ���� ����");
                _ = Task.Run(_ReadAsync);


                if (enterLobbyPacket != null)
                    enterLobbyPacket.Send_C_EnterLobby();
            }
        }

        public void Enter_Game()
        {
            if (enterEnterGamePacket != null)
            {
                enterEnterGamePacket.Send_C_EnterGame();
            }
        }

        public void Enter_Game_Finish()
        {
            if (enterEnterGameFinishPacket != null)
            {
                enterEnterGameFinishPacket.Send_C_EnterGameFinish();
            }
        }

        public void Send(IMessage packet)
        {
            ushort size = (ushort)(packet.CalculateSize());
            byte[] sendBuffer = new byte[size + PacketHeader.GetHeaderSize()];
            Array.Copy(BitConverter.GetBytes(size + PacketHeader.GetHeaderSize()), 0, sendBuffer, 0, sizeof(ushort));

            ushort protocolld = PacketIdFactory.GetPacketId(packet.GetType().Name);
            Array.Copy(BitConverter.GetBytes(protocolld), 0, sendBuffer, PacketHeader.GetIdStartPos(), sizeof(ushort));

            Array.Copy(packet.ToByteArray(), 0, sendBuffer, PacketHeader.GetHeaderSize(), size);

            _stream.Write(sendBuffer, 0, sendBuffer.Length);
        }

        private async Task _ReadAsync()
        {
            try
            {
                while (_isConnected)
                {
                    byte[] headerBuffer = new byte[PacketHeader.GetHeaderSize()];
                    await _Read(_stream, headerBuffer, PacketHeader.GetHeaderSize());

                    ushort totalSize = BitConverter.ToUInt16(headerBuffer, 0);
                    ushort packetId = BitConverter.ToUInt16(headerBuffer, PacketHeader.GetIdStartPos());

                    Debug.Log($"�޽��� ũ��: {totalSize}, �������� ID: {packetId}");

                    int packetSize = totalSize - PacketHeader.GetHeaderSize();
                    byte[] dataBuffer = new byte[packetSize];
                    await _Read(_stream, dataBuffer, packetSize);

                    // ��Ŷ ó���� MainThread ���� ����
                    await UniTask.SwitchToMainThread();

                    _ProcessPacket(packetId, dataBuffer);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"������ ���� �� ���� �߻�: {ex.Message}");
                _isConnected = false;
            }
        }

        private async Task _Read(NetworkStream stream, byte[] buffer, int size)
        {
            int bytesReadTotal = 0;

            while (bytesReadTotal < size)
            {
                int bytesRead = await stream.ReadAsync(buffer, bytesReadTotal, size - bytesReadTotal);
                if (bytesRead == 0)
                {
                    throw new Exception("�������� ������ ���������ϴ�.");
                }

                bytesReadTotal += bytesRead;
            }
        }

        private void _ProcessPacket(ushort protocolId, byte[] data)
        {
            try
            {
                PacketHandler packetHandler = new PacketHandler();
                packetHandler.ProcessHandler(protocolId, data);
            }
            catch (Exception ex)
            {
                Debug.LogError($"�޽��� ó�� �� ���� �߻�: {ex.Message}");
            }
        }
    }
}
