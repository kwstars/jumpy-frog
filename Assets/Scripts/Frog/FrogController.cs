using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the player character (frog) movement and animations.
/// Handles jump mechanics and input processing.
/// </summary>
public class FrogController : MonoBehaviour
{
    #region Enums
    /// <summary>
    /// Represents the possible movement directions for the frog
    /// </summary>
    private enum Direction
    {
        None, Left, Right, Up
    }
    #endregion

    #region Serialized Fields
    [Header("Jump Settings")]
    [Tooltip("The vertical height for a single jump")]
    [SerializeField] private float jumpDistance = 2.1f;

    [Tooltip("The speed at which the frog moves during jumps")]
    [SerializeField] private float jumpMovementSpeed = 0.134f;
    #endregion

    #region Private Fields
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Vector2 _targetPosition;
    private float _currentJumpDistance;
    private bool _isJumpButtonHeld;
    private bool _isJumping;
    private bool _shouldTriggerJump;
    private Vector2 _touchPosition;
    private Direction dir = Direction.None;
    #endregion

    #region Unity Lifecycle Methods
    /// <summary>
    /// Initialize components and variables.
    /// </summary>
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Handles animation triggers.
    /// </summary>
    private void Update()
    {
        if (_shouldTriggerJump)
        {
            TriggerJumpAnimation();
            _shouldTriggerJump = false;
        }
    }

    /// <summary>
    /// Handles physics-based movement.
    /// </summary>
    private void FixedUpdate()
    {
        if (_isJumping)
            _rigidbody.position = Vector2.Lerp(transform.position, _targetPosition, jumpMovementSpeed);
    }
    #endregion

    #region Input Methods
    /// <summary>
    /// Processes regular jump input.
    /// </summary>
    /// <param name="context">Input action callback context</param>
    public void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (context.performed && !_isJumping)
        {
            _currentJumpDistance = jumpDistance;
            _shouldTriggerJump = true;
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
            _currentJumpDistance = jumpDistance * 2;
            _isJumpButtonHeld = true;
        }

        if (context.canceled && _isJumpButtonHeld)
        {
            _isJumpButtonHeld = false;
            _shouldTriggerJump = true;
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
            _touchPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            var offset = (_touchPosition - (Vector2)transform.position).normalized;
            if (Mathf.Abs(offset.x) <= 0.7f)
            {
                dir = Direction.Up;
            }
            else if (offset.x < 0)
            {
                dir = Direction.Left;
            }
            else if (offset.x > 0)
            {
                dir = Direction.Right;
            }

            Debug.Log("Direction: " + dir);
        }
    }
    #endregion

    #region Animation Methods
    /// <summary>
    /// Triggers the jump animation.
    /// </summary>
    private void TriggerJumpAnimation()
    {
        switch (dir)
        {
            case Direction.Up:
                _targetPosition = new Vector2(transform.position.x, transform.position.y + _currentJumpDistance);
                break;
            case Direction.Left:
                _targetPosition = new Vector2(transform.position.x - _currentJumpDistance, transform.position.y);
                break;
            case Direction.Right:
                _targetPosition = new Vector2(transform.position.x + _currentJumpDistance, transform.position.y);
                break;
        }
        _animator.SetTrigger("Jump");

    }

    /// <summary>
    /// Called by animation event when jump starts.
    /// </summary>
    public void OnJumpAnimationStarted()
    {
        _isJumping = true;
    }

    /// <summary>
    /// Called by animation event when jump completes.
    /// </summary>
    public void OnJumpAnimationCompleted()
    {
        _isJumping = false;
    }
    #endregion
}