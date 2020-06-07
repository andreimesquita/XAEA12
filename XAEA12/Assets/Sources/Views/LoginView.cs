using Sources.Photon;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        PhotonServer server = PhotonServer.Instance;
        server.OnMyColorChanged += OnOnMyColorChanged;
        server.JoinGame(_playerNameInputField.text);
    }

    private void OnOnMyColorChanged(byte myColor)
    {
        OnSuccessCallback();
    }

    private void OnSuccessCallback()
    {
        SceneManager.LoadScene("Lobby");
    }
}
