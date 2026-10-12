using UnityEngine;

public class HealthPickup : BasePickup
{
    [SerializeField] private int _healAmount = 20;
    protected override void OnPickup(ActiveWeapon activeWeapon)
    {
        PlayerHealth health = activeWeapon.GetComponentInParent<PlayerHealth>();
        if (health.CurrentHealth >= health.MaxHealth)
        {
            return;
        }
        health.Heal(_healAmount);
        Destroy(gameObject);
    }
}
