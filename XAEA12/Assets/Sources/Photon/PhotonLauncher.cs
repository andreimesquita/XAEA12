using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Sources.Photon
{
    public class PhotonLauncher : IConnectionCallbacks, ILobbyCallbacks, IInRoomCallbacks, IMatchmakingCallbacks, IDisposable, IOnEventCallback
    {
        private const int MaxPlayersPerRoom = 4;
        private String playerName;
 
    
        public PhotonLauncher(string userName)
        {
            playerName = userName;
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void Dispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void Connect()
        {
            ConnectToPhoton();
        }

        public void ConnectToPhoton()
        { 
            PhotonNetwork.AuthValues = new AuthenticationValues
            {
                AuthType = CustomAuthenticationType.Custom,
                UserId = playerName
            };
        
            Debug.Log($"Connecting to Photon as {playerName}");
            PhotonNetwork.ConnectUsingSettings();
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

        public void OnConnected()
        {
            Debug.Log("Connected to Photon.");
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
        }

        public void OnLeftLobby()
        {
            throw new System.NotImplementedException();
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Room List updated, connecting to Random Room.");
            //Debug.Log($"Available rooms: {roomList.ToString()}");
            JoinRandomRoom();
        }
        
        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            throw new System.NotImplementedException();
        } 

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player {newPlayer.NickName} joined the room.");
            throw new System.NotImplementedException();
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("Player has left the room, game must be ended.");
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
            
            // set color and format
            
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

        public void OnEvent(EventData photonEvent)
        {
            Debug.Log($"Received Event: {photonEvent}");
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
