using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public int CurrentAmmo => _currentWeaponSO.CurrentAmmo;
    public int MagazineSize => _currentWeaponSO.MagazineSize;
    public int MaxReserveAmmo => MagazineSize * 3;

    [SerializeField] private WeaponSO _startingWeapon;
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private GameObject _zoomVignette;
    [SerializeField] private GameObject _crossHair;
    [SerializeField] private TMP_Text _ammoText;

    private PlayerMovement _playerMovement;
    private WeaponSO _currentWeaponSO;
    private Weapon _currentWeapon;
    private Animator _animator;

    private float _nextFireTime;
    private int _currentWeaponIndex = -1;

    private readonly List<WeaponSO> _ownedWeapons = new();
    private readonly List<Weapon> _weaponInstances = new();

    //Handle the single fire and weapon scrolling events
    private void OnEnable()
    {
        _playerMovement.OnFirePressed += HandleFirePressed;
        _playerMovement.OnWeaponScroll += HandleWeaponScroll;
        _playerMovement.OnReloadPressed += Reload;
    }
    private void OnDisable()
    {
        _playerMovement.OnFirePressed -= HandleFirePressed;
        _playerMovement.OnWeaponScroll -= HandleWeaponScroll;
        _playerMovement.OnReloadPressed -= Reload;
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponentInParent<PlayerMovement>();

    }
    private void Start()
    {
        if (_startingWeapon != null)
        {
            AddWeapon(_startingWeapon);
            AdjustAmmo(_startingWeapon.MagazineSize);

            _currentWeaponSO.CurrentReserveAmmo = MaxReserveAmmo;
            UpdateAmmoUI();
        }
    }
    private void Update()
    {
        AutoShooting();

    }
    public void AdjustAmmo(int amount)
    {
        _currentWeaponSO.CurrentAmmo += amount;

        _currentWeaponSO.CurrentAmmo = Mathf.Clamp(_currentWeaponSO.CurrentAmmo, 0, _currentWeaponSO.MagazineSize);


        UpdateAmmoUI();
    }
    public void AdjustReserveAmmo(int ammo)
    {
        _currentWeaponSO.CurrentReserveAmmo += ammo;
        _currentWeaponSO.CurrentReserveAmmo = Mathf.Clamp(_currentWeaponSO.CurrentReserveAmmo, 0, MaxReserveAmmo);
        UpdateAmmoUI();
    }

    private void Reload()
    {
        int ammoNeeded = _currentWeaponSO.MagazineSize - _currentWeaponSO.CurrentAmmo;

        if (ammoNeeded <= 0) return;
        if (_currentWeaponSO.CurrentReserveAmmo <= 0) return;

        int ammoToReload = Mathf.Min(ammoNeeded, _currentWeaponSO.CurrentReserveAmmo);

        _currentWeaponSO.CurrentAmmo += ammoToReload;
        _currentWeaponSO.CurrentReserveAmmo -= ammoToReload;

        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        _ammoText.text =
            $"{_currentWeaponSO.CurrentAmmo:D2}/{_currentWeaponSO.CurrentReserveAmmo:D2}";
    }
    //Handle the auto fire
    private void AutoShooting()
    {
        if (_playerMovement.IsShoting && Time.time >= _nextFireTime && _currentWeaponSO.FireMode == FireMode.FullAuto)
        {
            HandleShoot();
            _nextFireTime = Time.time + _currentWeaponSO.FireRate;
        }
        HandleZoom();
    }
    private void HandleZoom()
    {
        MeshRenderer[] _SniperMesh = _currentWeapon.GetComponentsInChildren<MeshRenderer>();
        if (_playerMovement.IsZooming)
        {
            switch (_currentWeaponSO.ZoomMode)
            {
                case ZoomMode.WithScope:
                    _currentWeapon.Zoom(_currentWeaponSO, _cinemachineCamera);
                    _zoomVignette.SetActive(true);
                    _crossHair.SetActive(false);

                    foreach (var mesh in _SniperMesh)
                    {
                        mesh.enabled = false;
                    }
                    _playerMovement.CameraRotationSpeed = _currentWeaponSO.ZoomCameraRotationSpeed;
                    break;
                case ZoomMode.WithoutScope:
                    _currentWeapon.Zoom(_currentWeaponSO, _cinemachineCamera);
                    break;
            }
        }
        else
        {
            _currentWeapon.ResetZoom(_cinemachineCamera);
            _zoomVignette.SetActive(false);
            _crossHair.SetActive(true);
            foreach (var mesh in _SniperMesh)
            {
                mesh.enabled = true;
            }
        }
    }

    private void HandleShoot()
    {
        if (_currentWeaponSO.CurrentAmmo > 0)
        {
            _currentWeapon.Shoot(_currentWeaponSO);
            _animator.Play("Shoot", 0, 0f);
            AdjustAmmo(-1);
        }
    }
    private void HandleFirePressed()
    {
        if (_currentWeaponSO.FireMode == FireMode.SemiAuto &&
            Time.time >= _nextFireTime)
        {
            HandleShoot();
            _nextFireTime = Time.time + _currentWeaponSO.FireRate;
        }
    }
    public void AddWeapon(WeaponSO newWeaponSO)
    {
        if (_ownedWeapons.Contains(newWeaponSO))
            return;

        Weapon newWeapon = Instantiate(newWeaponSO.WeaponPrefab, transform).GetComponent<Weapon>();

        newWeapon.gameObject.SetActive(false);
        newWeaponSO.CurrentAmmo = newWeaponSO.MagazineSize;
        newWeaponSO.CurrentReserveAmmo = newWeaponSO.MagazineSize * 3;

        _ownedWeapons.Add(newWeaponSO);
        _weaponInstances.Add(newWeapon);

        if (_currentWeaponIndex == -1)
        {
            SwitchWeapon(0);
        }
    }
    public void SwitchWeapon(int index)
    {
        if (_ownedWeapons.Count == 0)
            return;

        if (_currentWeaponIndex >= 0)
        {
            _weaponInstances[_currentWeaponIndex].gameObject.SetActive(false);
        }

        _currentWeaponIndex =
            (index + _ownedWeapons.Count) % _ownedWeapons.Count;

        _currentWeapon = _weaponInstances[_currentWeaponIndex];
        _currentWeaponSO = _ownedWeapons[_currentWeaponIndex];

        _currentWeapon.gameObject.SetActive(true);

        UpdateAmmoUI();
    }

    private void HandleWeaponScroll(float scrollValue)
    {
        if (scrollValue > 0)
        {
            SwitchWeapon(_currentWeaponIndex + 1);
        }
        else if (scrollValue < 0)
        {
            SwitchWeapon(_currentWeaponIndex - 1);
        }
    }

}
