using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isJumping;
    private bool _isZooming;
    public bool IsShoting { get; set; }
    public float CameraRotationSpeed
    {
        get { return _cameraRotationSpeed; }
        set { _cameraRotationSpeed = value; }
    }
    public float WeaponScroll { get; private set; }
    public bool IsZooming => _isZooming;
    public event Action OnFirePressed;
    public event Action<float> OnWeaponScroll;
    public event Action OnReloadPressed;

    [Header("Player movement seetings")]
    [SerializeField] private float _moveingSpeed;
    [SerializeField] private float _verticalVelocity;
    [SerializeField] private float _gravityScale;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _sprintSpeed;

    [Header("Camera settings")]
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private float _cameraRotationSpeed;
    [SerializeField] private float _maxRange = 90f;
    [SerializeField] private float _minRange = -90f;

    private bool _isSprinting;
    private float _xRotation;

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isJumping = true;
        }

    }
    public void OnZoom(InputAction.CallbackContext context)
    {

        _isZooming = context.ReadValueAsButton();

    }
    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        // For full-auto fire
        IsShoting = context.ReadValueAsButton();
        // For semi-auto fire
        if (context.performed)
        {
            OnFirePressed?.Invoke();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        _isSprinting = context.ReadValueAsButton();

    }

    public void OnScrollWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        float scrollValue =
            context.ReadValue<Vector2>().y;

        if (scrollValue != 0)
            OnWeaponScroll?.Invoke(scrollValue);
    }
    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnReloadPressed?.Invoke();
        }
    }
    #region Unity callbacks
    private void Update()
    {

        PlayerMoving();
        PlayerRotation();
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

    }
    #endregion

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void PlayerMoving()
    {
        Vector3 move =
                transform.right * _moveInput.x +
                transform.forward * _moveInput.y;

        Vector3 moveDirection = move * (_isSprinting ? _sprintSpeed : _moveingSpeed) * Time.deltaTime;


        if (_characterController.isGrounded)
        {
            _verticalVelocity = -1f;
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * _gravityScale * Time.deltaTime;
        }

        if (_isJumping && _characterController.isGrounded)
        {
            _verticalVelocity = Mathf.Sqrt(2f * _jumpHeight * Mathf.Abs(Physics.gravity.y * _gravityScale));
            _isJumping = false;
        }

        moveDirection.y = _verticalVelocity * Time.deltaTime;
        _characterController.Move(moveDirection);
    }

    private void PlayerRotation()
    {
        float mouseX = _lookInput.x * _cameraRotationSpeed * Time.deltaTime;
        float mouseY = _lookInput.y * _cameraRotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minRange, _maxRange);

        _cameraPivot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }


}
