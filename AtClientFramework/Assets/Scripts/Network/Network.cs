using System;
using System.Net.Sockets;
using UnityEngine;
using Google.Protobuf;
using System.Threading.Tasks;
using Assets.Scripts.Network.Handler;
using Unity.VisualScripting;
using Assets.Scripts.Network.Packet.Room;


namespace Assets.Scripts.Network
{
    public class Network : MonoBehaviour
    {
        private static Network _instance;

        private TcpClient     _socketConnection;
        private NetworkStream _stream;
        private bool          _isConnected = false;


        // public Dropdown       mode;
        // public InputField     inputIp;
        // public InputField     inputPort;


        public static Network Instance
        {
            get
            {
                if ( _instance == null )
                {
                    Network singletonObject = new Network();
                    _instance = singletonObject.AddComponent< Network >();
                    DontDestroyOnLoad( singletonObject );
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if ( _instance != null && _instance != this )
            {
                Destroy( gameObject );
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad( gameObject );
            }
        }

        public async void ConnectToTcpServer()
        {
            // if ( mode.options[ mode.value ].text == "Single" )
            // {
            //     Debug.Log( "SingleMode Start" );
            //     return;
            // }

            if ( _isConnected )
            {
                Debug.Log( "Already Connected" );
                return;
            }

            try
            {
               // string ip = inputIp.text;
               // int port = 0;
               // if ( int.TryParse( inputPort.text, out int intValue ) )
               //     port = intValue;

                string ip   = "192.168.25.22";
                int    port = 3333;

                _socketConnection = new TcpClient( ip, port );
                _stream = _socketConnection.GetStream();
                _isConnected = true;
            }
            catch ( Exception e )
            {
                Debug.LogError( "On client connect exception " + e );
            }

            if ( _isConnected )
            {
                Debug.Log( "서버 연결 성공, 수신 시작" );
                _ = Task.Run( _ReadAsync );

                Packet_C_EnterLobby enterLobby = GetComponent< Packet_C_EnterLobby >();

                if ( enterLobby != null )
                    enterLobby.Send_C_EnterLobby();
            }
        }

        public void Send( IMessage packet )
        {
            ushort size = (ushort)( packet.CalculateSize() );
            byte[] sendBuffer = new byte[ size + PacketHeader.GetHeaderSize() ];
            Array.Copy( BitConverter.GetBytes( size + PacketHeader.GetHeaderSize() ), 0, sendBuffer, 0, sizeof( ushort ) );

            ushort protocolld = PacketIdFactory.GetPacketId( packet.GetType().Name );
            Array.Copy( BitConverter.GetBytes( protocolld ), 0, sendBuffer, PacketHeader.GetIdStartPos(), sizeof( ushort ) );

            Array.Copy( packet.ToByteArray(), 0, sendBuffer, PacketHeader.GetHeaderSize(), size );

            _stream.Write( sendBuffer, 0, sendBuffer.Length );
        }

        private async Task _ReadAsync()
        {
            try
            {
                while ( _isConnected )
                {
                    byte[] headerBuffer = new byte[ PacketHeader.GetHeaderSize() ];
                    await _Read( _stream, headerBuffer, PacketHeader.GetHeaderSize() );

                    ushort totalSize = BitConverter.ToUInt16( headerBuffer, 0 );
                    ushort packetId = BitConverter.ToUInt16( headerBuffer, PacketHeader.GetIdStartPos() );

                    Debug.Log( $"메시지 크기: {totalSize}, 프로토콜 ID: {packetId}" );

                    int packetSize = totalSize - PacketHeader.GetHeaderSize();
                    byte[] dataBuffer = new byte[ packetSize ];
                    await _Read( _stream, dataBuffer, packetSize );

                    _ProcessPacket( packetId, dataBuffer );
                }
            }
            catch ( Exception ex )
            {
                Debug.LogError( $"데이터 수신 중 오류 발생: {ex.Message}" );
                _isConnected = false;
            }
        }

        private async Task _Read( NetworkStream stream, byte[] buffer, int size )
        {
            int bytesReadTotal = 0;

            while ( bytesReadTotal < size )
            {
                int bytesRead = await stream.ReadAsync( buffer, bytesReadTotal, size - bytesReadTotal );
                if ( bytesRead == 0 )
                {
                    throw new Exception( "서버와의 연결이 끊어졌습니다." );
                }

                bytesReadTotal += bytesRead;
            }
        }

        private void _ProcessPacket( ushort protocolId, byte[] data )
        {
            try
            {
                PacketHandler packetHandler = new PacketHandler();
                packetHandler.ProcessHandler( protocolId, data );
            }
            catch ( Exception ex )
            {
                Debug.LogError( $"메시지 처리 중 오류 발생: {ex.Message}" );
            }
        }
    }
}
