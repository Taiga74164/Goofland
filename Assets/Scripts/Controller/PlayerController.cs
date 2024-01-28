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
        public float runSpeedMultiplier = 1.5f;
        public float crouchSpeedMultiplier = 0.5f;
        public float jumpHeight = 5.0f;
        [Tooltip("Higher value, faster fall.")]
        public float fallMultiplier = 5.0f;
        [Tooltip("Lower value, shorter jump.")]
        public float lowJumpMultiplier = 2.0f;
    
        [Header("Coyote Time Settings")]
        public float coyoteTime = 0.2f;
        private float _coyoteTimeCounter;
        
        [Header("Jump Buffering Settings")]
        public float jumpBufferTime = 0.2f;
        private float _jumpBufferCounter;

        [Header("Ground Check Settings")]
        public Transform groundCheckTransform;
        public float groundCheckRadius = 0.1f;
        public LayerMask groundLayerMask;

        [Header("Player Health Settings")]
        public int maxHealth = 3;
        private int _currentHealth;
        
        [Header("Animation Settings")]
        public Animator animator;
        
        [Header("Audio Settings")]
        [SerializeField] private AudioSource jumpSound;
        [SerializeField] private AudioSource hurtSound;

        [Header("Event Settings")]
        public GameEvent onJump;
        public GameEvent onTakeDamage;
        #region Input Actions

        private InputAction _move, _jump, _crouch, _run, _attack;
    
        #endregion

        private Rigidbody2D _rb;
        // private WeaponController _weaponController;
        private PieController _pieController; 
        private Vector2 _moveInput = Vector2.zero;
        private bool _isMoving, _isRunning, _isCrouching, _isJumping, _isFalling, _isAttacking;
        private Vector3 _spawnPosition;

        #region Cached Properties

        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        #endregion

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
            _jump.started += _ =>
            {
                Jump();
                _isJumping = true;
            };
            _attack.started += _ =>
            {
                Attack(); 
                _isAttacking = true;
                
            };
            _attack.canceled += _ =>
            {
                _pieController.HandlePieThrow();
                _isAttacking = false;
            };
            
            // Set the player's health.
            _currentHealth = maxHealth;
            
            // Set the spawn position.
            _spawnPosition = transform.position;
        }

        private void Update()
        {
            if (GameManager.IsPaused) return;
            
            if (_attack.IsPressed())
                _pieController.Charging();
            
            HandleCoyoteTime();
            HandleJumpBuffering();

            HandleAnimations();
        }

        private void FixedUpdate()
        {
            if (GameManager.IsPaused) return;
            
            HandleMovement();
            HandleClampFallSpeed();
        }

        private void HandleCoyoteTime() => _coyoteTimeCounter = IsGrounded() ? 
            coyoteTime : _coyoteTimeCounter - Time.deltaTime;
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleJumpBuffering()
        {
            // Decrement the jump buffer counter.
            _jumpBufferCounter -= Time.deltaTime;
            // If the jump button is pressed, set the jump buffer counter.
            if (_jump.triggered)
                _jumpBufferCounter = jumpBufferTime;

            // If the jump buffer counter is greater than 0 and the coyote time counter is greater than 0, jump.
            if (!(_jumpBufferCounter > 0.0f) || !(_coyoteTimeCounter > 0.0f)) return;
            Jump();
            
            // Reset the jump buffer counter and coyote time counter.
            _jumpBufferCounter = 0.0f;
            _coyoteTimeCounter = 0.0f;
        }

        private void HandleAnimations()
        {
            animator.SetBool(Walking, _isMoving);
            animator.SetBool(Running, _isRunning);
            animator.SetBool(Jumping, _isJumping);
            animator.SetBool(Falling, _isFalling);
            animator.SetBool(Attacking, _isAttacking);
        }
        
        private void HandleMovement()
        {
            // Get the input values.
            _moveInput = _move.ReadValue<Vector2>();
            // Update the player's state.
            _isMoving = _moveInput != Vector2.zero;
            // Update the player's running state.
            _isRunning = _run.IsPressed() && _isMoving;
            // Update the player's crouching state.
            _isCrouching = _crouch.IsPressed();
            // Update the player's falling state.
            _isFalling = _rb.velocity.y < 0.0f;
            
            // Update the player's sprite based on the direction they are facing.
            switch (_moveInput.x)
            {
                // right
                case > 0:
                    animator.transform.eulerAngles = Vector3.zero;
                    break;
                // left
                case < 0:
                    animator.transform.eulerAngles = new Vector3(0, 180, 0);
                    break;
            }
            
            // Move the player.
            _rb.velocity = new Vector2(
                _moveInput.x * 
                (movementSpeed * 
                 (_isRunning && !_isCrouching ? runSpeedMultiplier : 1) *
                 (_isCrouching && IsGrounded() ? crouchSpeedMultiplier : 1)), 
                _rb.velocity.y);
        }
        
        private void HandleClampFallSpeed()
        {
            switch (_rb.velocity.y)
            {
                // If the player is falling, increase the fall speed.
                case < 0:
                    // Apply the fall multiplier.
                    _rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
                    _isJumping = false;
                    break;
                // If the player is jumping and the jump button is released, decrease the jump speed.
                case > 0 when !_jump.IsPressed():
                    // Apply the low jump multiplier.
                    _rb.velocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
                    break;
            }
        }

        /// <summary>
        /// Invoked when the jump action is performed.
        /// </summary>
        private void Jump()
        {
            if (GameManager.IsPaused && !IsGrounded() && _coyoteTimeCounter <= 0.0f) return;
            
            if (!IsGrounded() && _coyoteTimeCounter <= 0.0f) return;

            //Play Jump SFX

            onJump.Raise(this, jumpSound);

            // Jump.

            _rb.velocity = new Vector2(_rb.velocity.x, jumpHeight);
            
            // Animate the jump.
        }
        
        /// <summary>
        /// Invoked when the crouch action is performed.
        /// </summary>
        private void Crouch()
        {
            if (!IsGrounded() || _isRunning) return;
            
            // Animate the crouch.
        }
    
        /// <summary>
        /// Invoked when the Attack action is performed.
        /// </summary>
        private void Attack()
        {
            if (GameManager.IsPaused) return;
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

        public void TakeDamage(int damage = 1)
        {
            _currentHealth -= damage;
            onTakeDamage.Raise(this, hurtSound);

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
            if (_currentHealth <= 0)
                LevelManager.RestartLevel();
            
            _currentHealth = maxHealth;
            
            // Reset.
            transform.position = _spawnPosition;
        }

        private bool IsGrounded() => !GameManager.IsPaused &&
            Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayerMask);
    }
}
