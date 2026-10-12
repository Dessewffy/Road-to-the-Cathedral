using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public GameObject WeaponPrefab;
    public string WeaponName;
    public int Damage = 10;
    public float FireRate = 0.1f;
    public ParticleSystem HitVFX;
    public float Range = 100f;
    public AudioClip ShootSFX;
    public FireMode FireMode;
    public ZoomMode ZoomMode;
    public float ZoomFOV = 50f;
    public float ZoomCameraRotationSpeed = 10f;
    public int MagazineSize = 7;
    public int CurrentAmmo;
    public int CurrentReserveAmmo;

}

public enum FireMode
{
    SemiAuto,
    FullAuto
}
public enum ZoomMode
{
    WithoutScope,
    WithScope
}

