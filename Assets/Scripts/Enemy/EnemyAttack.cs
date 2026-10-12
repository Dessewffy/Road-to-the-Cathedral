using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    private Animator _enemyAnimator;
    private NavMeshAgent _enemyAgent;
    private float _lastAttackTime;
    private int _lastAttackType = -1;

    [SerializeField] private LayerMask _playerLayer;
    private float _attackRadius = 2f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private int _attackDamage = 50;


    private void Awake()
    {
        _enemyAnimator = GetComponentInChildren<Animator>();
        _enemyAgent = GetComponent<NavMeshAgent>();

    }
    private void Update()
    {
        Attack();
    }
    private void Attack()
    {
        if (Time.time < _lastAttackTime + _attackCooldown)
            return;

        _lastAttackTime = Time.time;
        if (_enemyAgent?.remainingDistance <= _enemyAgent?.stoppingDistance)
        {
            int attackType = GetRandomAttackType();

            _enemyAnimator.SetInteger("AttackType", attackType);
            _enemyAnimator.SetTrigger("Attack");

        }
    }

    private int GetRandomAttackType()
    {
        int attackType;

        do
        {
            attackType = Random.Range(0, 3);
        }
        while (attackType == _lastAttackType);

        _lastAttackType = attackType;
        return attackType;
    }


    public void DealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            _attackRadius,
            _playerLayer
        );

        foreach (Collider hit in hits)
        {
            IDamagable damageable = hit.GetComponent<IDamagable>();

            if (damageable != null)
            {
                damageable.TakeDamage(_attackDamage);
            }
        }
    }

}
