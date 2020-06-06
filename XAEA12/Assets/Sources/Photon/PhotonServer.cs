using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Sources.Common.Pattern;
using UnityEngine;
using Object = System.Object;

namespace Sources.Photon
{
    public class PhotonServer : IConnectionCallbacks, ILobbyCallbacks, IInRoomCallbacks, IMatchmakingCallbacks, IDisposable, IOnEventCallback
    {
        private readonly Dictionary<int, Action<EventData>> _onEventReceive = new Dictionary<int, Action<EventData>>();
            
        private readonly GameEventHelper _eventHelper;
        
        private const int MaxPlayersPerRoom = 4;
        
        private PhotonGameState _photonGameState;

        private bool _isMaster;
        
        public PhotonServer()
        {
            _eventHelper = new GameEventHelper();
            PhotonNetwork.AddCallbackTarget(this);
            
            _onEventReceive.Add((int) EVENT_CODES.SET_COLOR, OnReceiveColorSetEvent);
            _onEventReceive.Add((int) EVENT_CODES.PLAYER_READY, OnReceivePlayerReadyEvent);
            _onEventReceive.Add((int) EVENT_CODES.PRESS_BUTTON, OnReceiveButtonPressEvent);
            _onEventReceive.Add((int) EVENT_CODES.PLAYER_JOINED, OnReceiveColorSetEvent);
            _onEventReceive.Add((int) EVENT_CODES.ROOM_IS_FILLED, OnReceiveRoomIsFilledEvent);
            _onEventReceive.Add((int) EVENT_CODES.ALL_PLAYERS_READY, OnReceiveAllPlayersReadyEvent);
            _onEventReceive.Add((int) EVENT_CODES.GAME_STARTED, OnReceiveGameStartedEvent);
            _onEventReceive.Add((int) EVENT_CODES.TURN_STARTED, OnReceiveTurnStartedEvent);
            _onEventReceive.Add((int) EVENT_CODES.TURN_FINISHED, OnReceiveTurnFinishedEvent);
            _onEventReceive.Add((int) EVENT_CODES.TURN_RESET, OnReceiveTurnResetEvent);
        }

        public void Dispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this); 
        }

        public void SendEventToMaster(EVENT_CODES eventCode, Object content)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
            SendEvent(eventCode, content, raiseEventOptions);
        }

        public void SendEventToPlayer(EVENT_CODES eventCode, Object content)
        {
            // how to send only to player?
            SendEvent(eventCode, content, RaiseEventOptions.Default);
        }
        
        public void SendEventToServer(EVENT_CODES eventCode, Object content) 
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendEvent(eventCode, content, raiseEventOptions);
        }

        public void SendEvent(EVENT_CODES eventCode, Object content, RaiseEventOptions raiseEventOptions)
        {
            SendOptions sendOptions = new SendOptions { Reliability = true };
            Debug.Log($"Sending Event: {_eventHelper.GetEventCode(eventCode)}, {content}");
            PhotonNetwork.RaiseEvent(_eventHelper.GetEventCode(eventCode), content, raiseEventOptions, sendOptions);
        }
        
        public void OnEvent(EventData photonEvent)
        {
            if (_eventHelper.GetEventByCode(photonEvent.Code) != EVENT_CODES.INVALID)
            {
                Debug.Log($"Received Event: {_eventHelper.GetEventByCode(photonEvent.Code)}, {photonEvent.CustomData}");
                
                if (_onEventReceive.TryGetValue(55, out Action<EventData> callback))
                {
                    callback(photonEvent);
                }
            }
        }

        private void OnReceivePlayerJoinedEvent(EventData photonEvent)
        {
            throw new NotImplementedException();
        }

        private void OnReceiveTurnFinishedEvent(EventData photonEvent)
        {
            // Receive action
            int action = (int) photonEvent.CustomData;
        }

        private void OnReceiveTurnStartedEvent(EventData photonEvent)
        {
            // Start turn
        }

        private void OnReceiveGameStartedEvent(EventData photonEvent)
        {
            // Start the game
        }

        private void OnReceiveAllPlayersReadyEvent(EventData photonEvent)
        {
            // Start Game
        }
        
        private void OnReceiveRoomIsFilledEvent(EventData photonEvent)
        {
            // Enable UI to set ready
        }
        
        private void OnReceiveTurnResetEvent(EventData photonEvent)
        {
            // Enable UI to set ready
        }

        private void OnReceiveButtonPressEvent(EventData photonEvent)
        {
            int actorNumber = PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender).ActorNumber;
            _photonGameState.ButtonPress(actorNumber);

            if (_photonGameState.ActionComplete())
            {
                int action = _photonGameState.CurrentSelection;
                if (PatternMapperSO.Instance.IsValidPattern(action))
                {
                    SendTurnFinishedEvent(action);
                }
                else
                {
                    SendTurnResetEvent();
                }
            }
        }

        public void OnReceiveColorSetEvent(EventData photonEvent)
        {
            _photonGameState.SetPlayerColor(PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender).ActorNumber, 
                (byte) photonEvent.CustomData);
        }
        
        public void OnReceivePlayerReadyEvent(EventData photonEvent)
        {
            _photonGameState.SetPlayerReady(PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender).ActorNumber);

            // Send global event if everyone ready
            if (_photonGameState.AllPlayersReady())
            {
                SendAllPlayersReadyEvent();
                SendTurnStartedEvent(); 
            }
        }

        public void SendSetColorEvent(byte color)
        {
            SendEventToMaster(EVENT_CODES.SET_COLOR, color); 
        } 
        
        public void SendPlayerReadyEvent()
        {
            SendEventToMaster(EVENT_CODES.PLAYER_READY, null);
        }
        
        public void SendButtonPressEvent()
        {
            SendEventToMaster(EVENT_CODES.PRESS_BUTTON, null);
        }
        
        public void SendRoomIsFilledEvent()
        {
            SendEventToServer(EVENT_CODES.ROOM_IS_FILLED, null);
        }
        
        public void SendAllPlayersReadyEvent()
        {
            SendEventToServer(EVENT_CODES.ALL_PLAYERS_READY, null);
        }
        
        public void SendTurnStartedEvent()
        {
            _photonGameState.ResetTurn();
            SendEventToServer(EVENT_CODES.TURN_STARTED, null);
        }
        
        public void SendTurnFinishedEvent(int action)
        {
            SendEventToServer(EVENT_CODES.TURN_FINISHED, action);
        }
        
        public void SendTurnResetEvent()
        {
            SendEventToServer(EVENT_CODES.TURN_RESET, null);
        }

        public void JoinGame(string userName)
        {
            PhotonNetwork.AuthValues = new AuthenticationValues
            {
                AuthType = CustomAuthenticationType.Custom,
                UserId = userName
            };
            
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                Debug.Log($"Connecting to Photon as {userName}");
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public void JoinRandomRoom()
        {
            MatchmakingMode matchmakingMode = MatchmakingMode.FillRoom;
            TypedLobby lobby = PhotonNetwork.CurrentLobby;
            PhotonNetwork.JoinRandomRoom(null, MaxPlayersPerRoom, matchmakingMode, lobby, null);
        }
    
        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom("test-room", new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        }
        
        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player {newPlayer.ActorNumber} joined the room.");
            
            _photonGameState.AddPlayer(newPlayer);
            if (_photonGameState.RoomIsFilled())
            {
                SendRoomIsFilledEvent();
            }
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("Player has left the room, game must be ended.");
        }
        

        public void OnConnected()
        {
            //Debug.Log("Connected to Photon.");
        }

        private bool IsMasterClient()
        {
            return PhotonNetwork.IsMasterClient;
        }

        public void OnConnectedToMaster()
        {

            Debug.Log("Connected to Master.");
            PhotonNetwork.JoinLobby();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected: {cause.ToString()}");
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            //Debug.Log("Region List received....");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            throw new System.NotImplementedException();
        }
        
        public void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby.");
            JoinRandomRoom();
        }

        public void OnLeftLobby()
        {
            throw new System.NotImplementedException();
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            //Debug.Log("Room List updated, connecting to Random Room.");
            //Debug.Log($"Available rooms: {roomList.ToString()}");
        }
        
        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            throw new System.NotImplementedException();
        } 

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            throw new System.NotImplementedException();
        }

        public void OnCreatedRoom()
        {
            //Debug.Log("Created Room successfully");
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log($"Failed to Create Room: {message}");
        }

        public void OnJoinedRoom()
        {
            Debug.Log("Joined room :D");
            
            if (IsMasterClient())
            {
                Debug.Log("Starting game state..");
                _photonGameState = new PhotonGameState();
                _photonGameState.AddPlayer(PhotonNetwork.LocalPlayer);
            }
            
            SendSetColorEvent(GameEventHelper.Red);
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"Failed to Join Room: {message}");
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"Failed to Join Room: {message}, creating new");
            CreateRoom();
        }

        public void OnLeftRoom()
        {
            Debug.Log("Left room :(");
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            throw new System.NotImplementedException();
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            throw new System.NotImplementedException();
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            throw new System.NotImplementedException();
        }
    }
}
