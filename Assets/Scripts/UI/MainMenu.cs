using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void ApplicationQuit()
    {
        Debug.Log("Application is quitting.");
        Application.Quit();
    }
    public void StartGame()
    {

        SceneManager.LoadScene("First");
    }
}
