using Sources.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Views
{
    public sealed class LobbyView : MonoBehaviour
    {
        private void Start()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            photonFacade.OnLobbyAllPlayersReady += OnLobbyAllPlayersReady;
        }

        private void OnLobbyAllPlayersReady()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            photonFacade.OnLobbyAllPlayersReady -= OnLobbyAllPlayersReady;
            SceneManager.LoadScene("Game");
        }
    }
}
