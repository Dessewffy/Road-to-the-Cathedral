using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private RaycastHit _hit;

    [SerializeField] private Light _muzzleFlashLight;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private ParticleSystem _blodVFX;
    [SerializeField] private LayerMask _interactableLayer;

    private CinemachineImpulseSource _cinemachineImpulseSource;

    private void Awake()
    {
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void Shoot(WeaponSO weaponSO)
    {

        _muzzleFlash.Play();
        _muzzleFlashLight.enabled = true;
        AudioSource.PlayClipAtPoint(weaponSO.ShootSFX, transform.position);
        _cinemachineImpulseSource.GenerateImpulse();
        StartCoroutine(DisableMuzzleLight());

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _hit, weaponSO.Range, _interactableLayer, QueryTriggerInteraction.Ignore))
        {

            if (_hit.collider.CompareTag("Enemy"))
            {

                EnemyHealth enemyHealth = _hit.collider.GetComponent<EnemyHealth>();
                Instantiate(_blodVFX, _hit.point, Quaternion.LookRotation(_hit.normal));
                enemyHealth?.TakeDamage(weaponSO.Damage);
            }
            else
            {
                Instantiate(weaponSO.HitVFX, _hit.point, Quaternion.LookRotation(Camera.main.transform.forward));
            }


        }

    }
    public void Zoom(WeaponSO weaponSO, CinemachineCamera _cinemachineCamera)
    {
        _cinemachineCamera.Lens.FieldOfView = weaponSO.ZoomFOV;
    }

    public void ResetZoom(CinemachineCamera _cinemachineCamera)
    {
        _cinemachineCamera.Lens.FieldOfView = 60f;
    }
    private IEnumerator DisableMuzzleLight()
    {
        yield return new WaitForSeconds(0.05f);
        _muzzleFlashLight.enabled = false;
    }
}
