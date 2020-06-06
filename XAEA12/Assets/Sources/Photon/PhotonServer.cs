using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Sources.Photon
{
    public class PhotonServer : IConnectionCallbacks, ILobbyCallbacks, IInRoomCallbacks, IMatchmakingCallbacks, IDisposable, IOnEventCallback
    {
        private const int MaxPlayersPerRoom = 4;
        private PhotonGameState _photonGameState;
        private readonly GameEventHelper _eventHelper;
        private bool isMaster;
        
 
    
        public PhotonServer()
        {
            _eventHelper = new GameEventHelper();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void Dispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void SendEvent(EVENT_CODES eventCode, String content)
        {
            Debug.Log($"Sending Event: {_eventHelper.GetEventCode(eventCode)}, {content}");
            PhotonNetwork.RaiseEvent(
                _eventHelper.GetEventCode(eventCode), 
                content, 
                RaiseEventOptions.Default,
                SendOptions.SendReliable);
        }
        
        public void OnEvent(EventData photonEvent)
        {
            if (_eventHelper.GetEventByCode(photonEvent.Code) != EVENT_CODES.INVALID)
            {
                Debug.Log($"Received Event: {photonEvent}, {photonEvent.CustomData}");
            }
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
            Debug.Log($"Player {newPlayer.NickName} joined the room.");
            _photonGameState.AddPlayer(newPlayer);
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

            if (IsMasterClient())
            {
                _photonGameState = new PhotonGameState();
                _photonGameState.AddPlayer(PhotonNetwork.LocalPlayer);
            }
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
            SendEvent(EVENT_CODES.SET_COLOR, "red");
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
