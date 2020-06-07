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
        public readonly PhotonGameState GameState = new PhotonGameState();
        public bool AmIReady => GameState.IsPlayerReady(PhotonNetwork.LocalPlayer.ActorNumber);
        public int CurrentPattern { get; private set; }
        
        // UI callbacks
        public event Action OnGamePlayerDead;
        public event Action OnLoginMyColorChanged;
        /// <summary>
        /// This event ist triggered at the LOBBY when all players are on "player ready" state.
        /// </summary>
        public event Action OnLobbyAllPlayersReady;
        /// <summary>
        /// This event is triggered at the GAME scene when all game scenes are loaded and the game's simulation is ready to be started.
        /// </summary>
        public event Action OnLobbyPlayerReadyStateChanged;
        public event Action OnLobbyPlayerListChanged;
        public event Action OnRoomIsFilledEvent;
        public event Action OnStartGameSimulation;
        public event Action<bool> OnButtonActiveStateChanged;
        public event Action<int> OnPatternChanged;
        public event Action<string> OnTriggerAnimation;
        
        private PhotonFacade()
        {
            _eventHelper = new GameEventHelper();
            _eventDispatcher = new PhotonEventDispatcher(_eventHelper);
            PhotonNetwork.AddCallbackTarget(this);
            
            // MasterClient
            _onEventReceive.Add((int) EVENT_CODES.NOTIFY_CLIENT_READY, OnMasterReceivePlayerReadyEvent);
            _onEventReceive.Add((int) EVENT_CODES.CLIENT_PRESSED_BUTTON, OnMasterReceiveButtonPressEvent);
            _onEventReceive.Add((int) EVENT_CODES.LOADED_CLIENT_GAME_SCENE, OnMasterReceiveGameSceneLoadedEvent);
            
            // All Clients
            _onEventReceive.Add((int) EVENT_CODES.CHARACTER_KILLED, OnReceiveCharacterDeadEvent);
            _onEventReceive.Add((int) EVENT_CODES.ROOM_IS_FILLED, OnReceiveRoomIsFilledEvent);
            _onEventReceive.Add((int) EVENT_CODES.ALL_PLAYERS_READY, OnReceiveAllPlayersReadyEvent);
            _onEventReceive.Add((int) EVENT_CODES.GAME_STARTED, OnReceiveGameStartedEvent);
            
            _onEventReceive.Add((int) EVENT_CODES.PATTERN_RESET, OnReceivePatternResetEvent);
            _onEventReceive.Add((int) EVENT_CODES.PATTERN_CHANGED, OnReceivePatternChangedEvent);
            _onEventReceive.Add((int) EVENT_CODES.PATTERN_TRIGGER_ANIMATION, OnReceiveTriggerPatternAnimationEvent);
        }

        private void OnReceivePatternResetEvent(EventData data)
        {
            CurrentPattern = 0;
            OnPatternChanged?.Invoke(CurrentPattern);
            OnButtonActiveStateChanged?.Invoke(true);
        }

        private void OnReceiveTriggerPatternAnimationEvent(EventData data)
        {
            CurrentPattern = (int) data.CustomData;
            OnPatternChanged?.Invoke(CurrentPattern);
            string trigger = PatternMapperSO.Instance.GetAnimationTriggerByPattern(CurrentPattern);
            OnTriggerAnimation?.Invoke(trigger);
        }
        
        private void OnReceivePatternChangedEvent(EventData data)
        {
            CurrentPattern = (int) data.CustomData;
            OnPatternChanged?.Invoke(CurrentPattern);
        }

        ~PhotonFacade()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        
        public override void OnEvent(EventData photonEvent)
        {
            Debug.Log($"Received Event: {photonEvent.Code}, {photonEvent.CustomData}");
                
            if (_onEventReceive.TryGetValue(photonEvent.Code, out Action<EventData> callback))
            {
                callback(photonEvent);
            }
        }
        
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PhotonGameState.PLAYER_COLOR_FIELD))
            {
                if (targetPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    OnLoginMyColorChanged?.Invoke();
                }
                else
                {
                    OnLobbyPlayerListChanged?.Invoke();
                }
            }
        }

        private void OnReceiveGameStartedEvent(EventData photonEvent)
        {
            // All game scenes are loaded - We can start the game
            OnStartGameSimulation?.Invoke();
        }

        private void OnReceiveAllPlayersReadyEvent(EventData photonEvent)
        {
            // Load game scene for all players
            OnLobbyAllPlayersReady?.Invoke();
        }
        
        private void OnReceiveRoomIsFilledEvent(EventData photonEvent)
        {
            // Enable UI to set ready
            GameState.SetRoomFilled();
            OnRoomIsFilledEvent?.Invoke();
        }

        /// <summary>
        /// Trigger death animation.
        /// </summary>
        private void OnReceiveCharacterDeadEvent(EventData photonEvent)
        {
            OnGamePlayerDead?.Invoke();
        }
        
        private void OnMasterReceiveGameSceneLoadedEvent(EventData photonEvent)
        {
            GameState.SetPlayerGameSceneLoaded(photonEvent.Sender);
            if (GameState.AreAllGameScenesLoaded())
            {
                SendGameStartedEvent();
            }
        }
        
        private void OnMasterReceiveButtonPressEvent(EventData photonEvent)
        {
            int actorNumber = photonEvent.Sender;
            GameState.OnButtonPress(actorNumber);

            int pattern = GameState.PatternMask;
            if (GameState.ActionComplete())
            {
                if (PatternMapperSO.Instance.IsValidPattern(pattern))
                {
                    SendTriggerPatternAnimation(pattern);
                }
                else
                {
                    SendResetPatternEvent();
                }
            }
            else
            {
                SendPatternChangedEvent(pattern);
            }
        }

        private void OnMasterReceivePlayerReadyEvent(EventData photonEvent)
        {
            Room room = PhotonNetwork.CurrentRoom;
            Player player = room.GetPlayer(photonEvent.Sender);
            Debug.Log($"'{player.UserId}' is ready!");
            GameState.SetPlayerReady(photonEvent.Sender);
            OnLobbyPlayerReadyStateChanged?.Invoke();

            // Send global event if everyone ready
            if (GameState.AllPlayersReady())
            {
                SendAllPlayersReadyEvent();
            }
        }

#region LOBBY USER INTERACTIONS        
        
        public void TrySendPlayerReadyEvent()
        {
            if (!GameState.IsRoomFilled()) return;
            int localUserId = PhotonNetwork.LocalPlayer.ActorNumber;
            GameState.SetPlayerReady(localUserId);
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
            GameState.SetPlayerGameSceneLoaded(localUserId);
            _eventDispatcher.SendEventToMaster(EVENT_CODES.LOADED_CLIENT_GAME_SCENE, null);
        }
        
        /// <summary>
        /// This method must be called during GAME.
        /// </summary>
        public void SendButtonPressEvent()
        {
            OnButtonActiveStateChanged?.Invoke(false);
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

        private void SendResetPatternEvent()
        {
            GameState.ResetPatternMask();
            _eventDispatcher.SendEventToServer(EVENT_CODES.PATTERN_RESET, null);
        }

        private void SendGameStartedEvent()
        {
            GameState.ResetPatternMask();
            _eventDispatcher.SendEventToServer(EVENT_CODES.GAME_STARTED, null);
        }

        private void SendTriggerPatternAnimation(int pattern)
        {
            _eventDispatcher.SendEventToServer(EVENT_CODES.PATTERN_CHANGED, pattern);
        }
        
        private void SendPatternChangedEvent(int pattern)
        {
            _eventDispatcher.SendEventToServer(EVENT_CODES.PATTERN_CHANGED, pattern);
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
                JoinRandomRoom();
            }
            else
            {
                Debug.Log($"Connecting to Photon as {userName}");
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        private void JoinRandomRoom()
        {
            var roomOptions = new RoomOptions {PublishUserId = true};
            PhotonNetwork.JoinOrCreateRoom("XAEA12", roomOptions, TypedLobby.Default);
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
                byte playerColor = GameState.GetUnusedColor();
                Debug.Log($"Player {newPlayer.ActorNumber} color is {playerColor}");
                SetPlayerColor(newPlayer, playerColor);
                if (GameState.RoomIsFilled())
                {
                    SendRoomIsFilledEvent();
                }
            }
            
            void SetPlayerColor(Player player, byte color)
            {
                var hashtable = new Hashtable {{PhotonGameState.PLAYER_COLOR_FIELD, color}};
                player.SetCustomProperties(hashtable);
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
                byte color = GameState.GetUnusedColor();
                Player player = PhotonNetwork.LocalPlayer;
                var hashtable = new Hashtable {{PhotonGameState.PLAYER_COLOR_FIELD, color}};
                player.SetCustomProperties(hashtable);
                Debug.Log($"My color is {color}");
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
