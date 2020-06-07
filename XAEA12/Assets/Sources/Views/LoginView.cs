using Sources.Photon;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sources.Views
{
    public class LoginView : MonoBehaviour
    {
        [SerializeField]
        public TextMeshProUGUI _playerNameInputField;
    
        public void OnConfirmButtonPressed()
        {
            if (string.IsNullOrEmpty(_playerNameInputField.text))
            {
                Debug.LogError("Sorry burb! No 'null' value or empty spaces allowed!  :)");
                return;
            }

            PhotonFacade server = PhotonFacade.Instance;
            server.OnLoginMyColorChanged += OnLoginOnLoginMyColorChanged;
            server.Login(_playerNameInputField.text);
        }

        private void OnLoginOnLoginMyColorChanged()
        {
            PhotonFacade.Instance.OnLoginMyColorChanged -= OnLoginOnLoginMyColorChanged;
            OnSuccessCallback();
        }

        private void OnSuccessCallback()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
