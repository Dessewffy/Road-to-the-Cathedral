using UnityEngine;

// Inheritance
public class AmmoPickup : BasePickup
{
    [SerializeField] private int _ammoAmount = 100;
    // POLYMORPHISM
    protected override void OnPickup(ActiveWeapon activeWeapon)
    {

        activeWeapon.AdjustReserveAmmo(_ammoAmount);
        Destroy(gameObject);

    }

}
