using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginView : MonoBehaviour
{
    public void OnConfirmButtonPressed()
    {
        OnSuccessCallback();
    }

    private void OnSuccessCallback()
    {
        SceneManager.LoadScene("Lobby");
    }
}
