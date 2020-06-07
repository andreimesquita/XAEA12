using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Sources.Common.Pattern;
using UnityEngine;

namespace Sources.Photon
{
    public sealed class PhotonFacade : IPhotonFacade, ILobbyUserInteractions, IGameUserInteractions
    {
        private static PhotonFacade _instance;
        public static PhotonFacade Instance => _instance ?? (_instance = new PhotonFacade());
        
        private readonly Dictionary<int, Action<EventData>> _onEventReceive = new Dictionary<int, Action<EventData>>();
            
        private readonly GameEventHelper _eventHelper;
        private readonly PhotonEventDispatcher _eventDispatcher;
        private const int MaxPlayersPerRoom = 4;
        private bool _isMaster;
        private byte _myColor;
        public PhotonGameState GameState;
        public bool IsMyColorSet => _myColor != 0;
        public bool AmIReady => GameState._playersReadyById[PhotonNetwork.LocalPlayer.ActorNumber];
        
        // UI callbacks
        public event Action OnGamePlayerDead;
        public event Action<byte> OnLoginMyColorChanged;
        /// <summary>
        /// This event ist triggered at the LOBBY when all players are on "player ready" state.
        /// </summary>
        public event Action OnLobbyAllPlayersReady;
        /// <summary>
        /// This event is triggered at the GAME scene when all game scenes are loaded and the game's simulation is ready to be started.
        /// </summary>
        public event Action OnGameTurnStarted;
        public event Action OnLobbyPlayerReadyStateChanged;
        public event Action<bool> OnGameButtonActiveStateChanged;
        
        private PhotonFacade()
        {
            _eventHelper = new GameEventHelper();
            _eventDispatcher = new PhotonEventDispatcher(_eventHelper);
            PhotonNetwork.AddCallbackTarget(this);
            
            // MasterClient
            _onEventReceive.Add((int) EVENT_CODES.SET_CLIENT_COLOR, OnClientReceiveColorSetEvent);
            _onEventReceive.Add((int) EVENT_CODES.NOTIFY_CLIENT_READY, OnMasterReceivePlayerReadyEvent);
            _onEventReceive.Add((int) EVENT_CODES.CLIENT_PRESSED_BUTTON, OnMasterReceiveButtonPressEvent);
            _onEventReceive.Add((int) EVENT_CODES.LOADED_CLIENT_GAME_SCENE, OnMasterReceiveGameSceneLoadedEvent);
            
            // All Clients
            _onEventReceive.Add((int) EVENT_CODES.CHARACTER_KILLED, OnReceiveCharacterDeadEvent);
            _onEventReceive.Add((int) EVENT_CODES.ROOM_IS_FILLED, OnReceiveRoomIsFilledEvent);
            _onEventReceive.Add((int) EVENT_CODES.ALL_PLAYERS_READY, OnReceiveAllPlayersReadyEvent);
            _onEventReceive.Add((int) EVENT_CODES.GAME_STARTED, OnReceiveGameStartedEvent);
            _onEventReceive.Add((int) EVENT_CODES.TURN_STARTED, OnReceiveTurnStartedEvent);
            _onEventReceive.Add((int) EVENT_CODES.TURN_FINISHED, OnReceiveTurnFinishedEvent);
            _onEventReceive.Add((int) EVENT_CODES.TURN_RESET, OnReceiveTurnResetEvent);
        }

        ~PhotonFacade()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        
        public override void OnEvent(EventData photonEvent)
        {
            EVENT_CODES eventCode = _eventHelper.GetEventByCode(photonEvent.Code);
            if (eventCode != EVENT_CODES.INVALID)
            {
                Debug.Log($"Received Event: {_eventHelper.GetEventByCode(photonEvent.Code)}, {photonEvent.CustomData}");
                
                if (_onEventReceive.TryGetValue((int)eventCode, out Action<EventData> callback))
                {
                    callback(photonEvent);
                }
            }
        }

        private void OnReceiveTurnFinishedEvent(EventData photonEvent)
        {
            // Receive action
            int action = (int) photonEvent.CustomData;
            //TODO(andrei)
        }

        private void OnReceiveTurnStartedEvent(EventData photonEvent)
        {
            OnGameTurnStarted?.Invoke();
        }

        private void OnReceiveGameStartedEvent(EventData photonEvent)
        {
            // All game scenes are loaded - We can start the game
            //TODO(andrei)
        }

        private void OnReceiveAllPlayersReadyEvent(EventData photonEvent)
        {
            // Start Game
            OnLobbyAllPlayersReady?.Invoke();
        }
        
        private void OnReceiveRoomIsFilledEvent(EventData photonEvent)
        {
            // Enable UI to set ready
            //TODO(andrei)
        }

        /// <summary>
        /// Trigger death animation.
        /// </summary>
        private void OnReceiveCharacterDeadEvent(EventData photonEvent)
        {
            OnGamePlayerDead?.Invoke();
        }
        
        private void OnReceiveTurnResetEvent(EventData photonEvent)
        {
            // Enable UI to set ready
            //TODO(andrei)
        }

        private void OnMasterReceiveGameSceneLoadedEvent(EventData photonEvent)
        {
            GameState.SetGameSceneReady(photonEvent.Sender);
            if (GameState.AreAllGameScenesLoaded())
            {
                SendGameStartedEvent();
            }
        }
        
        private void OnMasterReceiveButtonPressEvent(EventData photonEvent)
        {
            int actorNumber = photonEvent.Sender;
            GameState.ButtonPress(actorNumber);

            if (GameState.ActionComplete())
            {
                int action = GameState.CurrentSelection;
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

        private void OnClientReceiveColorSetEvent(EventData photonEvent)
        {
            _myColor = (byte) photonEvent.CustomData;
            OnLoginMyColorChanged?.Invoke(_myColor);
        }

        private void OnMasterReceivePlayerReadyEvent(EventData photonEvent)
        {
            Debug.Log($"'{GameState._playersById[photonEvent.Sender].UserId}' is ready!");
            GameState.SetPlayerReady(photonEvent.Sender, true);
            OnLobbyPlayerReadyStateChanged?.Invoke();

            // Send global event if everyone ready
            if (GameState.AllPlayersReady())
            {
                SendAllPlayersReadyEvent();
            }
        }

        private void SendSetColorEvent(int actorId, byte color)
        {
            _eventDispatcher.SendEventToPlayer(EVENT_CODES.SET_CLIENT_COLOR, color, actorId); 
        }

#region LOBBY USER INTERACTIONS        
        
        public void SendPlayerReadyEvent()
        {
            int localUserId = PhotonNetwork.LocalPlayer.ActorNumber;
            GameState.SetPlayerReady(localUserId, true);
            OnLobbyPlayerReadyStateChanged?.Invoke();
            _eventDispatcher.SendEventToMaster(EVENT_CODES.NOTIFY_CLIENT_READY, null);
        }

#endregion

#region GAME USER INTERACTIONS

        /// <summary>
        /// This method is meant to be called when the game scene has been loaded and everything is ready to start the game's turn.
        /// </summary>
        public void SendGameSceneLoadedEvent()
        {
            int localUserId = PhotonNetwork.LocalPlayer.ActorNumber;
            GameState.SetGameSceneReady(localUserId);
            _eventDispatcher.SendEventToMaster(EVENT_CODES.LOADED_CLIENT_GAME_SCENE, null);
        }
        
        /// <summary>
        /// This method must be called during GAME.
        /// </summary>
        public void SendButtonPressEvent()
        {
            OnGameButtonActiveStateChanged?.Invoke(false);
            _eventDispatcher.SendEventToMaster(EVENT_CODES.CLIENT_PRESSED_BUTTON, null); 
        }

        /// <summary>
        /// This must be called by the 'MasterClient' only. It triggers the DEATH animation for all players.
        /// </summary>
        public void SendCharacterIsDeadEvent()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            _eventDispatcher.SendEventToServer(EVENT_CODES.CHARACTER_KILLED, null);
        }
        
#endregion

        private void SendRoomIsFilledEvent()
        {
            _eventDispatcher.SendEventToServer(EVENT_CODES.ROOM_IS_FILLED, null);
        }

        private void SendAllPlayersReadyEvent()
        {
            _eventDispatcher.SendEventToServer(EVENT_CODES.ALL_PLAYERS_READY, null);
        }

        private void SendTurnStartedEvent()
        {
            GameState.ResetTurn();
            _eventDispatcher.SendEventToServer(EVENT_CODES.TURN_STARTED, null);
        }

        private void SendGameStartedEvent()
        {
            GameState.ResetTurn();
            _eventDispatcher.SendEventToServer(EVENT_CODES.GAME_STARTED, null);
        }

        private void SendTurnFinishedEvent(int action)
        {
            _eventDispatcher.SendEventToServer(EVENT_CODES.TURN_FINISHED, action);
        }

        private void SendTurnResetEvent()
        {
            _eventDispatcher.SendEventToServer(EVENT_CODES.TURN_RESET, null);
        }

        public void Login(string userName)
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

        private void JoinRandomRoom()
        {
            MatchmakingMode matchmakingMode = MatchmakingMode.FillRoom;
            TypedLobby lobby = PhotonNetwork.CurrentLobby;
            PhotonNetwork.JoinRandomRoom(null, MaxPlayersPerRoom, matchmakingMode, lobby, null);
        }

        private void CreateRoom()
        {
            PhotonNetwork.CreateRoom("test-room", new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player {newPlayer.ActorNumber} joined the room.");

            if (PhotonNetwork.IsMasterClient)
            {
                byte playerColor = GameState.AddPlayer(newPlayer);
                Debug.Log($"Player {newPlayer.ActorNumber} color is {playerColor}");
                SendSetColorEvent( newPlayer.ActorNumber, playerColor);
                
                if (GameState.RoomIsFilled())
                {
                    SendRoomIsFilledEvent();
                }
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master.");
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected: {cause.ToString()}");
            Application.Quit(0);
        }
        
        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby.");
            JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined room :D");
            
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Starting game state..");
                GameState = new PhotonGameState();
                _myColor = GameState.AddPlayer(PhotonNetwork.LocalPlayer);
                OnLoginMyColorChanged?.Invoke(_myColor);
                
                Debug.Log($"My color is {_myColor}");
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"Failed to Join Room: {message}, creating new");
            CreateRoom();
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Left room :(");
            Application.Quit(0);
        }
    }
}
