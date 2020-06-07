using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;

namespace Sources.Photon
{
    public abstract class IPhotonFacade : IConnectionCallbacks, ILobbyCallbacks, IInRoomCallbacks, IMatchmakingCallbacks, IOnEventCallback
    {
        public abstract void OnJoinedLobby();
        public virtual void OnLeftLobby() { }
        public virtual void OnRoomListUpdate(List<RoomInfo> roomList) { }
        public virtual void OnCustomAuthenticationFailed(string debugMessage) { } 
        public virtual void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) { }
        public virtual void OnFriendListUpdate(List<FriendInfo> friendList) {}
        public virtual void OnCreatedRoom() { }
        public virtual void OnCreateRoomFailed(short returnCode, string message) {}
        public abstract void OnJoinedRoom();
        public virtual void OnJoinRoomFailed(short returnCode, string message) { }
        public abstract void OnJoinRandomFailed(short returnCode, string message);
        public abstract void OnLeftRoom();
        public virtual void OnConnected() { }
        public abstract void OnConnectedToMaster();
        public abstract void OnDisconnected(DisconnectCause cause);
        public virtual void OnRegionListReceived(RegionHandler regionHandler) { }
        public virtual void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }
        public abstract void OnPlayerEnteredRoom(Player newPlayer);
        public virtual void OnPlayerLeftRoom(Player otherPlayer) { }
        public virtual void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {}
        public virtual void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {}
        public virtual void OnMasterClientSwitched(Player newMasterClient) {}
        public abstract void OnEvent(EventData photonEvent);
    }
}
