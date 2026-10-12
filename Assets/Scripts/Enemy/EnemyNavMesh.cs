using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    private Transform _playerTransform;
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;


    }

    private void Update()
    {
        EnemyMoving();
    }

    private void EnemyMoving()
    {


        if (!_navMeshAgent.GetComponent<EnemyHealth>().EnemyIsDead)
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
        }

    }
}
