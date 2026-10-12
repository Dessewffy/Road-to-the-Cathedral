using TMPro;
using UnityEngine;

public class Win : MonoBehaviour
{
    [SerializeField] private TMP_Text _enemiesRemainingText;
    [SerializeField] private GameObject _winScreen;
    public int EnemiesRemaining => _enemiesRemaining;
    private int _enemiesRemaining;
    const string ENEMIES_REMAINING_TEXT = "Enemies Remaining: ";
    public void UpdateEnemiesRemaining(int amount)
    {
        _enemiesRemaining += amount;
        _enemiesRemainingText.text = ENEMIES_REMAINING_TEXT + _enemiesRemaining.ToString();

        if (_enemiesRemaining <= 0)
        {
            ShowWinScreen();
        }
    }

    public void ShowWinScreen()
    {
        _winScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }
}