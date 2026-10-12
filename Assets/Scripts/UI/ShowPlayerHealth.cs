using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowPlayerHealth : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Image _damageOverlay;

    private Coroutine _damageIndicatorCoroutine;

    private void OnEnable()
    {
        _playerHealth.OnHealthChanged += UpdateHealthText;
    }

    private void OnDisable()
    {
        _playerHealth.OnHealthChanged -= UpdateHealthText;
    }

    private void UpdateHealthText(int health)
    {
        _healthText.text = health.ToString();
    }

    public void PlayerHealthChangeIndicator(Color color)
    {
        if (_damageIndicatorCoroutine != null)
        {
            StopCoroutine(_damageIndicatorCoroutine);
        }

        _damageIndicatorCoroutine = StartCoroutine(
            ShowDamageIndicator(color)
        );
    }

    private IEnumerator ShowDamageIndicator(Color color)
    {
        color.a = 0.4f;
        _damageOverlay.color = color;

        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            color.a = Mathf.Lerp(
                0.4f,
                0f,
                elapsed / duration
            );

            _damageOverlay.color = color;

            yield return null;
        }

        color.a = 0f;
        _damageOverlay.color = color;

        _damageIndicatorCoroutine = null;
    }
}