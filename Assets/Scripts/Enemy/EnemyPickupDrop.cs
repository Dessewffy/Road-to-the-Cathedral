using UnityEngine;

public class EnemyPickupDrop : MonoBehaviour
{
    [SerializeField] private GameObject[] _pickupWeaponsPrefabs;
    [SerializeField] private GameObject[] _pickupCommonPrefabs;
    private bool _hasDroppedPickup;
    private int _commonPickupChance = 75;
    public void DropPickup()
    {
        if (_hasDroppedPickup) return;
        _hasDroppedPickup = true;

        if (Random.Range(0, 100) >= _commonPickupChance)
        {
            int randomIndex = Random.Range(0, _pickupWeaponsPrefabs.Length);
            GameObject pickupPrefab = _pickupWeaponsPrefabs[randomIndex];
            Instantiate(pickupPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        }
        else
        {
            int randomIndex = Random.Range(0, _pickupCommonPrefabs.Length);
            GameObject pickupPrefab = _pickupCommonPrefabs[randomIndex];
            Instantiate(pickupPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }

}
