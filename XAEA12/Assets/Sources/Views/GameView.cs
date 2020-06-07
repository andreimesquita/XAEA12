using System.Collections;
using Photon.Pun;
using Sources.Common.Pattern;
using Sources.Photon;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sources.Views
{
    public sealed class GameView : MonoBehaviour
    {
        [SerializeField]
        private Button _playerButton;
        [SerializeField]
        private Animator _characterAnimator;
        [SerializeField]
        public UnityEvent _onSimulationStarted;
        [SerializeField]
        private GameObject[] _mainButtons = new GameObject[4];

        private IEnumerator Start()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            PhotonFacade.Instance.OnStartGameSimulation += OnStartGameSimulation;
            PhotonFacade.Instance.OnButtonActiveStateChanged += OnButtonActiveStateChanged;
            PhotonFacade.Instance.OnTriggerAnimation += OnTriggerAnimation;
            ActivatePlayerButton();
            yield return null;
            photonFacade.SendGameSceneLoadedEvent();
        }

        private void ActivatePlayerButton()
        {
            PhotonFacade photonFacade = PhotonFacade.Instance;
            photonFacade.GameState.TryGetPlayerColor(PhotonNetwork.LocalPlayer.ActorNumber, out byte color);
            int playerIndex = ColorPatternHelper.GetIndexByColorPattern(color);
            for (int i = 0; i < _mainButtons.Length; i++)
            {
                bool isPlayerButton = (playerIndex == i);
                _mainButtons[i].SetActive(isPlayerButton);
            }
        }
        
        private void OnTriggerAnimation(string trigger)
        {
            int hash = Animator.StringToHash(trigger);
            _characterAnimator.SetTrigger(hash);
        }

        private void OnButtonActiveStateChanged(bool isActive)
        {
            _playerButton.interactable = isActive;
        }

        private void OnStartGameSimulation()
        {
            _onSimulationStarted.Invoke();
        }
    }
}
