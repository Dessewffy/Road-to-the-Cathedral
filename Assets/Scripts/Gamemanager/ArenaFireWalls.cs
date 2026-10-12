using UnityEngine;

public class ArenaFireWalls : MonoBehaviour
{
    [SerializeField] private GameObject[] _fireWalls;
    private bool _isActive = false;
    private Win _win;
    private void Start()
    {
        _win = FindFirstObjectByType<Win>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_isActive && other.CompareTag("Player"))
        {
            foreach (var fireWall in _fireWalls)
            {
                fireWall.SetActive(true);
            }
            _isActive = true;

        }
        else if (_isActive && other.CompareTag("Player") && _win.EnemiesRemaining <= 0)
        {
            foreach (var fireWall in _fireWalls)
            {
                fireWall.SetActive(false);
            }
            _isActive = false;
        }

    }
}
