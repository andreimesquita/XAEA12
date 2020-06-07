using Photon.Pun;
using Photon.Realtime;
using Sources.Common.Pattern;
using Sources.Photon;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sources.Views
{
    public class LobbyView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI[] _playerNames = new TextMeshProUGUI[4];
        [SerializeField]
        private Button[] _playerButtons = new Button[4];
        
        private void Awake()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            photonFacade.OnLobbyPlayerReadyStateChanged += UpdateButtonsState;
            photonFacade.OnLobbyAllPlayersReady += OnLobbyAllPlayersReady;
            UpdateButtonsState();
        }

        private void UpdateButtonsState()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            int myActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

            for (int i = 0; i < 4; i++)
            {
                byte colorPattern = ColorPatternHelper.IndexToColorPattern(i);
                bool isMyButton = colorPattern == myActorNumber;
                if (isMyButton)
                {
                    _playerButtons[i].gameObject.SetActive(true);
                    _playerNames[i].text = PhotonNetwork.LocalPlayer.UserId;
                    continue;
                }
                bool hasPlayerInThisButton = photonFacade.GameState._colorsByPlayerId.ContainsKey(colorPattern);
                _playerButtons[i].gameObject.SetActive(hasPlayerInThisButton);
                _playerButtons[i].interactable = false;
                if (hasPlayerInThisButton)
                {
                    Player player = photonFacade.GameState._playerByColorPattern[colorPattern];
                    _playerNames[i].text = player.UserId;
                }
                else
                {
                    _playerNames[i].text = "NONE";
                }
            }
        }

        public void OnButtonPressed()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            if (!photonFacade.IsMyColorSet) return;
            if (photonFacade.AmIReady) return;
            photonFacade.SendPlayerReadyEvent();
        }

        private void OnLobbyAllPlayersReady()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            photonFacade.OnLobbyPlayerReadyStateChanged -= UpdateButtonsState;
            photonFacade.OnLobbyAllPlayersReady -= OnLobbyAllPlayersReady;
            SceneManager.LoadScene("Game");
        }
    }
}
