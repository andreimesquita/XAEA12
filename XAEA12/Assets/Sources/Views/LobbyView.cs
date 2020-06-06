using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyView : MonoBehaviour
{
    public void OnButtonPressed()
    {
        OnSuccessCallback();
    }

    private void OnSuccessCallback()
    {
        SceneManager.LoadScene("Game");
    }
}
