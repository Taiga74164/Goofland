using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Settings")]
        public float movementSpeed = 5.0f;
        public float runSpeedMultiplier = 2.0f;
        public float crouchSpeedMultiplier = 0.5f;
        public float jumpHeight = 5.0f;
    
        [Header("Ground Check Settings")]
        public Transform groundCheckTransform;
        public float groundCheckRadius = 0.1f;
        public LayerMask groundLayerMask;

        [Header("Player Health Settings")]
        public int maxHealth = 3;
        private int _currentHealth;
        
        #region Input Actions
    
        private InputAction _move, _jump, _crouch, _run, _attack;
    
        #endregion

        private Rigidbody2D _rb;
        // private WeaponController _weaponController;
        private PieController _pieController; 
        private Vector2 _moveInput = Vector2.zero;
        private bool _isMoving, _isRunning, _isCrouching;

        private void Awake()
        {
            if (GameManager.Instance.playerController == null)
                GameManager.Instance.playerController = this;
        }

        private void Start()
        {
            // Get the rigidbody component.
            _rb = GetComponent<Rigidbody2D>();
            
            // Get the weapon controller.
            // _weaponController = GetComponent<WeaponController>();
            _pieController = GetComponent<PieController>();
            
            // Set up input action references.
            _move = InputManager.Move;
            _jump = InputManager.Jump;
            _crouch = InputManager.Crouch;
            _run = InputManager.Run;
            _attack = InputManager.Attack;

            // Listen for input actions.
            _jump.started += Jump;
            _attack.started += Attack;
            _attack.canceled += _ => _pieController.HandlePieThrow();
            
            // Set the player's health.
            _currentHealth = maxHealth;
        }

        private void FixedUpdate()
        {
            if (_attack.IsPressed())
                _pieController.Charging(); 
            
            HandleMovement();
        }

        private void HandleMovement()
        {
            // Get the input values.
            _moveInput = _move.ReadValue<Vector2>();
            // Update the player's state.
            _isMoving = _moveInput != Vector2.zero;
            // Update the player's running state.
            _isRunning = _run.IsPressed();
            // Update the player's crouching state.
            _isCrouching = _crouch.IsPressed();
            
            // Move the player.
            _rb.velocity = new Vector2(
                _moveInput.x * 
                (movementSpeed * 
                 (_isRunning && !_isCrouching && IsGrounded() ? runSpeedMultiplier : 1) *
                 (_isCrouching && IsGrounded() ? crouchSpeedMultiplier : 1)), 
                _rb.velocity.y);
        }
    
        /// <summary>
        /// Invoked when the jump action is performed.
        /// </summary>
        /// <param name="context">The input context.</param>
        private void Jump(InputAction.CallbackContext context)
        {
            if (!IsGrounded()) return;
        
            // Jump.
            _rb.velocity = new Vector2(_rb.velocity.x, jumpHeight);
            
            // Animate the jump.
        }
        
        /// <summary>
        /// Invoked when the crouch action is performed.
        /// </summary>
        /// <param name="context">The input context.</param>
        private void Crouch(InputAction.CallbackContext context)
        {
            if (!IsGrounded() || _isRunning) return;
            
            // Animate the crouch.
        }
    
        /// <summary>
        /// Invoked when the Attack action is performed.
        /// </summary>
        /// <param name="context">The input context.</param>
        private void Attack(InputAction.CallbackContext context)
        {
           /*
            switch (_weaponController.currentWeapon)
            {
                case WeaponType.Pie:
                    _weaponController.SpawnWeapon(WeaponType.Pie);
                    // Animate the pie throw.
                    break;
                case WeaponType.WaterGun:
                    _weaponController.waterGun.Charge();
                    // Animate the water gun.
                    break;
                case WeaponType.BananaPeel:
                    _weaponController.SpawnWeapon(WeaponType.BananaPeel);
                    // Animate the banana peel throw.
                    break;
            }
           */
           _pieController.Charge();
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Die();
            }
            else
            {
                // TODO: Animate the player taking damage.
            }
        }

        private void Die()
        {
            // TODO: Animate the player dying.
            
            // TODO: Play the death sound.
            
            // TODO: HUD feed back.
            Respawn();
        }

        private void Respawn()
        {
            _currentHealth = maxHealth;
            
            // Reset or restart the level.
            transform.Reset(true, true);
        }

        private bool IsGrounded() => Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayerMask);
    }
}
