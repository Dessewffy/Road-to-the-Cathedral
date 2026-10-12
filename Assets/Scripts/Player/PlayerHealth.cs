using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    //ENCAPSULATION
    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    public Action<int> OnHealthChanged;

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private ShowPlayerHealth _showPlayerHealth;
    [SerializeField] private GameOver _gameOver;

    private int _currentHealth;
    private CinemachineImpulseSource _cinemachineImpulseSource;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _cinemachineImpulseSource.GenerateImpulse();
        _showPlayerHealth.PlayerHealthChangeIndicator(Color.red);
        OnHealthChanged?.Invoke(_currentHealth);


        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(int amount)
    {
        _currentHealth += amount;
        _showPlayerHealth.PlayerHealthChangeIndicator(Color.green);
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        OnHealthChanged?.Invoke(_currentHealth);

    }
    public void Die()
    {
        _gameOver.ShowGameOverScreen();
    }


}
