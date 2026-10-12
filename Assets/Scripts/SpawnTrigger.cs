using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    private bool _hasTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!_hasTriggered && other.CompareTag("Player"))
        {

            _hasTriggered = true;
            Spawner spawner = GetComponentInParent<Spawner>();
            spawner.SpawnEnemies();

        }
    }
}
