using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the player character (frog) movement and animations.
/// Handles jump mechanics and input processing.
/// </summary>
public class FrogController : MonoBehaviour
{
    #region Serialized Fields
    [Header("Jump Settings")]
    [Tooltip("The vertical height for a single jump")]
    [SerializeField] private float jumpHeight = 1f;

    [Tooltip("The speed at which the frog moves during jumps")]
    [SerializeField] private float jumpMovementSpeed = 0.134f;
    #endregion

    #region Private Fields
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Vector2 _targetPosition;
    private float _currentJumpHeight;
    private bool _isJumpButtonHeld;
    private bool _isJumping;
    private Vector2 _touchPosition;
    #endregion

    #region Unity Lifecycle Methods
    /// <summary>
    /// Initialize components and variables.
    /// </summary>
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Handles animation triggers.
    /// </summary>
    private void Update()
    {
        if (_targetPosition.y - transform.position.y < 0.1f)
        {
            _isJumping = false;
        }
    }

    /// <summary>
    /// Handles physics-based movement.
    /// </summary>
    private void FixedUpdate()
    {
        if (_isJumping)
        {
            _rigidbody.position = Vector2.Lerp(transform.position, _targetPosition, jumpMovementSpeed);
        }
    }
    #endregion

    #region Input Methods
    /// <summary>
    /// Processes regular jump input.
    /// </summary>
    /// <param name="context">Input action callback context</param>
    public void OnJumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Jump performed: " + context.performed);
        if (context.performed && !_isJumping)
        {
            _currentJumpHeight = jumpHeight;
            _targetPosition = new Vector2(transform.position.x, transform.position.y + _currentJumpHeight);
            _isJumping = true;
        }
    }

    /// <summary>
    /// Processes long jump input with button hold.
    /// </summary>
    /// <param name="context">Input action callback context</param>
    public void OnLongJumpPerformed(InputAction.CallbackContext context)
    {
        if (context.performed && !_isJumping)
        {
            _currentJumpHeight = jumpHeight * 2;
            _isJumpButtonHeld = true;
        }

        if (context.canceled && _isJumpButtonHeld)
        {
            _isJumpButtonHeld = false;
            _targetPosition = new Vector2(transform.position.x, transform.position.y + _currentJumpHeight);
            _isJumping = true;
        }
    }

    /// <summary>
    /// Captures the touch position for directional control.
    /// </summary>
    /// <param name="context">Input action callback context</param>
    public void OnTouchPositionInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _touchPosition = context.ReadValue<Vector2>();
            // Touch position can be used for additional controls if needed
        }
    }
    #endregion
}