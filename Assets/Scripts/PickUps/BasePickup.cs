using UnityEngine;
// ABSTRACTION
public abstract class BasePickup : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 50f;
    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0f, _rotationSpeed * Time.deltaTime, 0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActiveWeapon activeWeapon = other.GetComponentInChildren<ActiveWeapon>();

            OnPickup(activeWeapon);

        }
    }

    protected abstract void OnPickup(ActiveWeapon activeWeapon);
}
