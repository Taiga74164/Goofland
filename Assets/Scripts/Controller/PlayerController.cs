using Controller.States;
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
        public AudioSource audioSource;

        #region Properties
        
        public int CurrentHealth { get; private set; }
        
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public IdleState idleState;
        [HideInInspector] public WalkingState walkingState;
        [HideInInspector] public RunningState runningState;
        [HideInInspector] public CrouchingState crouchingState;
        [HideInInspector] public JumpingState jumpingState;
        [HideInInspector] public FallingState fallingState;

        
        private PlayerState _currentState;
        private PieController _pieController; 
        private Vector3 _spawnPosition;
        private InputAction _move, _jump, _crouch, _run, _attack;
        private bool _isAttacking;
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
            rb = GetComponent<Rigidbody2D>();
            
            // Get the pie controller component.
            _pieController = GetComponent<PieController>();
            
            // Set up the player's states.
            idleState = new IdleState(this);
            walkingState = new WalkingState(this);
            runningState = new RunningState(this);
            crouchingState = new CrouchingState(this);
            jumpingState = new JumpingState(this);
            fallingState = new FallingState(this);
            
            // Set the player's initial state.
            ChangeState(idleState);
        }

        private void Start()
        {
            // Set up input action references.
            SetupInputActions();

            // Listen for input actions.
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
            if (rb.velocity.y < -30.0f && transform.position.y < -30.0f) TakeDamage();
            
            if (GameManager.IsPaused) return;
            
            if (_attack.IsPressed())
                _pieController.Charging();
            
            _currentState.HandleInput();
            _currentState.UpdateState();
            // HandleCoyoteTimeAndJumpBuffering();
        }

        private void FixedUpdate()
        {
            if (GameManager.IsPaused) return;
            
            _currentState.FixedUpdateState();
        }

        public void ChangeState(PlayerState state)
        {
            _currentState?.ExitState();
            _currentState = state;
            _currentState.EnterState();
        }
        
        private void SetupInputActions()
        {
            // _move = InputManager.Move;
            _jump = InputManager.Jump;
            // _crouch = InputManager.Crouch;
            // _run = InputManager.Run;
            _attack = InputManager.Attack;
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

        /// <summary>
        /// Invoked when the jump action is performed.
        /// </summary>
        private void Jump()
        {
            if (GameManager.IsPaused && !IsGrounded() && _coyoteTimeCounter <= 0.0f) return;
            
            if (!IsGrounded() && _coyoteTimeCounter <= 0.0f) return;
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
            audioSource.Configure(playerSettings.fartSoundData);
            audioSource.Play();
            if (CurrentHealth <= 0)
            {
                Die();
            }
            else
            {
                rb.AddForce((transform.position - enemy!.position).normalized * playerSettings.knockbackForce);
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
        public bool IsGrounded() => !GameManager.IsPaused &&
                                     Physics2D.OverlapCircle(
                                         GameObject.FindWithTag("GroundCheck").transform.position, 
                                         playerSettings.groundCheckRadius, 
                                         playerSettings.groundLayerMask);
    }
}
