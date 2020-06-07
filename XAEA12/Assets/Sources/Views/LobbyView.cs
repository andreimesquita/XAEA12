using Sources.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Views
{
    public sealed class LobbyView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _tutorialPattern;
        
        private void Start()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            photonFacade.OnLobbyAllPlayersReady += OnLobbyAllPlayersReady;

            if (photonFacade.GameState.IsRoomFilled())
            {
                ShowTutorialPattern();
            }
            else
            {
                photonFacade.OnRoomIsFilledEvent += OnRoomIsFilledEvent;
            }
        }

        private void OnRoomIsFilledEvent()
        {
            ShowTutorialPattern();
        }

        private void ShowTutorialPattern()
        {
            _tutorialPattern.SetActive(true);
        }
        
        private void OnLobbyAllPlayersReady()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            photonFacade.OnLobbyAllPlayersReady -= OnLobbyAllPlayersReady;
            SceneManager.LoadScene("Game");
        }
    }
}
