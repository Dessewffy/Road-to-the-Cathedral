using UnityEngine;

public class ShowmainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuScreen;

    public void ShowMainMenuScreen()
    {
        _mainMenuScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void HideMainMenuScreen()
    {
        _mainMenuScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
}
