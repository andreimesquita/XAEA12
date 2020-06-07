using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Sources.Photon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Views
{
    public sealed class LobbyButtonView : MonoBehaviour
    {
        [SerializeField]
        private string _colorPattern = default;
        [SerializeField]
        private TextMeshProUGUI _playerName = default;
        [SerializeField]
        private Button _playerButton = default;
        
        private void Start()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            photonFacade.OnLobbyPlayerReadyStateChanged += UpdateSlotState;
            photonFacade.OnLobbyPlayerListChanged += UpdateSlotState;
            UpdateSlotState();
        }

        public void SendPlayerReadyEvent()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            photonFacade.TrySendPlayerReadyEvent();
        }
        
        private void UpdateSlotState()
        {
            byte colorPattern = (byte) Convert.ToInt32(_colorPattern, 2);
            if (TryGetPlayerByColorPattern(colorPattern, out Player target))
            {
                Player localPlayer = PhotonNetwork.LocalPlayer;
                bool isLocalPlayer = localPlayer.ActorNumber == target.ActorNumber;
                if (isLocalPlayer)
                {
                    bool isPlayerReady = PhotonFacade.Instance.GameState.IsPlayerReady(localPlayer.ActorNumber);
                    _playerButton.interactable = !isPlayerReady;
                }
                else
                {
                    _playerButton.interactable = false;
                }
                _playerName.text = target.UserId;
            }
            else
            {
                _playerButton.interactable = false;
                _playerName.text = "...";
            }
        }

        private bool TryGetPlayerByColorPattern(byte colorPattern, out Player target)
        {
            Room room = PhotonNetwork.CurrentRoom;
            Dictionary<int, Player>.KeyCollection actorIds = room.Players.Keys;
            foreach (int actorId in actorIds)
            {
                Player player = room.GetPlayer(actorId);
                Hashtable hashtable = player.CustomProperties;
                if (hashtable.ContainsKey(PhotonGameState.PLAYER_COLOR_FIELD))
                {
                    byte color = (byte) hashtable[PhotonGameState.PLAYER_COLOR_FIELD];
                    if (color == colorPattern)
                    {
                        target = player;
                        return true;
                    }
                }
            }
            target = null;
            return false;
        }
    }
}
