using System.Collections;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private int _maxHealth = 100;


    private Animator _enemyAnimator;
    private NavMeshAgent _enemyNavMesh;
    private CapsuleCollider _enemyCollider;
    private Win _win;
    private EnemyPickupDrop _enemyPickupDrop;
    private int _currentHealth;
    public bool EnemyIsDead => _currentHealth <= 0;

    private void Awake()
    {
        _enemyPickupDrop = GetComponent<EnemyPickupDrop>();
        _currentHealth = _maxHealth;
        _enemyAnimator = GetComponent<Animator>();
        _enemyNavMesh = GetComponent<NavMeshAgent>();
        _enemyCollider = GetComponent<CapsuleCollider>();

    }
    private void Start()
    {
        _win = FindFirstObjectByType<Win>();
        _win.UpdateEnemiesRemaining(1);

    }
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _win.UpdateEnemiesRemaining(-1);
            _enemyPickupDrop.DropPickup();
            Die();
        }
    }

    private void Die()
    {
        _enemyAnimator.SetTrigger("Die");
        _enemyNavMesh.enabled = false;
        _enemyCollider.enabled = false;

        StartCoroutine(DestroyAfterDeath());
    }

    private IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
