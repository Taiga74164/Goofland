using Managers;
using Objects.Scriptable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Settings")]
        public PlayerSettings playerSettings;
        
        [Header("Animation Settings")]
        public Animator animator;
        
        [Header("Audio Settings")]
        [SerializeField] private AudioData jumpSoundData;
        [SerializeField] private AudioData runSoundData;
        [SerializeField] private AudioData walkSoundData;
        [SerializeField] private AudioData fartSoundData;
        [SerializeField] private AudioSource audioSource;

        #region Properties
        
        public int CurrentHealth { get; private set; }
        
        private Rigidbody2D _rb;
        private PieController _pieController; 
        private Vector3 _spawnPosition;
        private Vector2 _moveInput = Vector2.zero;
        private InputAction _move, _jump, _crouch, _run, _attack;
        private bool _isMoving, _isRunning, _isCrouching, _isJumping, _isFalling, _isAttacking;
        private float _coyoteTimeCounter, _jumpBufferCounter;

        #endregion
        
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
            
            // Get the rigidbody component.
            _rb = GetComponent<Rigidbody2D>();
            
            // Get the pie controller component.
            _pieController = GetComponent<PieController>();
        }

        private void Start()
        {
            // Set up input action references.
            SetupInputActions();

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
            CurrentHealth = playerSettings.maxHealth;
            
            // Set the spawn position.
            _spawnPosition = transform.position;
        }

        private void Update()
        {
            // Take damage if the player falls too far.
            if (_rb.velocity.y < -30.0f && transform.position.y < -30.0f) TakeDamage();
            
            if (GameManager.IsPaused) return;
            
            if (_attack.IsPressed())
                _pieController.Charging();
            
            HandleInput();
            HandleSfx();
            HandleCoyoteTimeAndJumpBuffering();

            HandleAnimations();
        }

        private void FixedUpdate()
        {
            if (GameManager.IsPaused) return;
            
            HandleMovement();
            HandleClampFallSpeed();
        }
        
        private void SetupInputActions()
        {
            _move = InputManager.Move;
            _jump = InputManager.Jump;
            _crouch = InputManager.Crouch;
            _run = InputManager.Run;
            _attack = InputManager.Attack;
        }

        private void HandleInput()
        {
            // Get the input values.
            _moveInput = _move.ReadValue<Vector2>();
            // Update the player's state.
            _isMoving = _moveInput != Vector2.zero;
            // Update the player's running state.
            _isRunning = _run.IsPressed() && _isMoving && IsGrounded();
            // Update the player's crouching state.
            _isCrouching = _crouch.IsPressed() && IsGrounded();
            // Update the player's falling state.
            _isFalling = _rb.velocity.y < 0.0f;
        }
        
        private void HandleSfx()
        {
            switch (_isMoving)
            {
                // Play SFX based on the player's state.
                case true when !_isJumping && IsGrounded() && !audioSource.isPlaying:
                {
                    var audioData = _isRunning ? runSoundData : walkSoundData;
                    audioSource.Configure(audioData);
                    audioSource.Play();
                    break;
                }
                case false when !_isJumping:
                    audioSource.Stop();
                    break;
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleCoyoteTimeAndJumpBuffering()
        {
            // Decrement the coyote time counter if the player is grounded.
            _coyoteTimeCounter = IsGrounded() ? playerSettings.coyoteTime : _coyoteTimeCounter - Time.deltaTime;
            
            // Decrement the jump buffer counter.
            _jumpBufferCounter -= Time.deltaTime;
            // If the jump button is pressed, set the jump buffer counter.
            if (_jump.triggered)
                _jumpBufferCounter = playerSettings.jumpBufferTime;

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
            // Update the player's sprite based on the direction they are facing.
            var animatorTransform = animator.transform;
            animatorTransform.eulerAngles = _moveInput.x switch
            {
                // right
                > 0 => Vector3.zero,
                // left
                < 0 => new Vector3(0, 180, 0),
                _ => animatorTransform.eulerAngles
            };

            // Move the player.
            _rb.velocity = new Vector2(
                _moveInput.x * 
                (playerSettings.movementSpeed * 
                 (_isRunning && !_isCrouching ? playerSettings.runSpeedMultiplier : 1) *
                 (_isCrouching ? playerSettings.crouchSpeedMultiplier : 1)), 
                _rb.velocity.y);
        }
        
        private void HandleClampFallSpeed()
        {
            switch (_rb.velocity.y)
            {
                // If the player is falling, increase the fall speed.
                case < 0:
                    // Apply the fall multiplier.
                    _rb.velocity += Vector2.up * (Physics2D.gravity.y * (playerSettings.fallMultiplier - 1) * Time.deltaTime);
                    _isJumping = false;
                    break;
                // If the player is jumping and the jump button is released, decrease the jump speed.
                case > 0 when !_jump.IsPressed():
                    // Apply the low jump multiplier.
                    _rb.velocity += Vector2.up * (Physics2D.gravity.y * (playerSettings.lowJumpMultiplier - 1) * Time.deltaTime);
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
            
            // Play the jump sound.
            audioSource.Configure(jumpSoundData);
            audioSource.Play();
            
            // Increase the jump force if the player is running.
            var jumpForce = playerSettings.jumpHeight;
            if (_isRunning)
            {
                jumpForce *= playerSettings.jumpBoostMultiplier;
                Debug.Log("Jump Boost!");
            }

            // Jump.
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
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
            
           _pieController.Charge();
        }

        public void TakeDamage(Transform enemy = null, int damage = 1)
        {
            CurrentHealth -= damage;
            audioSource.Configure(fartSoundData);
            audioSource.Play();
            if (CurrentHealth <= 0)
            {
                Die();
            }
            else
            {
                _rb.AddForce((transform.position - enemy!.position).normalized * playerSettings.knockbackForce);
            }
        }

        private void Die()
        {
            Respawn();
        }

        private void Respawn()
        {
            if (CurrentHealth <= 0)
                LevelManager.RestartLevel();
            
            CurrentHealth = playerSettings.maxHealth;
            
            // Reset.
            transform.position = _spawnPosition;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private bool IsGrounded() => !GameManager.IsPaused &&
                                     Physics2D.OverlapCircle(
                                         GameObject.FindWithTag("GroundCheck").transform.position, 
                                         playerSettings.groundCheckRadius, 
                                         playerSettings.groundLayerMask);
    }
}
