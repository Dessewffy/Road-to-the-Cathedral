using UnityEngine;

public class WeaponPickup : BasePickup
{
    [SerializeField] private WeaponSO _weaponSO;
    protected override void OnPickup(ActiveWeapon activeWeapon)
    {
        if (activeWeapon != null)
        {
            activeWeapon.AddWeapon(_weaponSO);
        }
        Destroy(gameObject);
    }
}