using Controllers.StateMachines;
using Enemies;
using Managers;
using Objects.Scriptable;
using UnityEngine;
using Utils;

namespace Controllers
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
        public float CoyoteTimeCounter { get; private set; }
        public float JumpBufferCounter { get; private set; }
        
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public PieController pieController;
        [HideInInspector] public InputController inputController;
        [HideInInspector] public IdleState idleState;
        [HideInInspector] public WalkingState walkingState;
        [HideInInspector] public RunningState runningState;
        [HideInInspector] public CrouchingState crouchingState;
        [HideInInspector] public JumpingState jumpingState;
        [HideInInspector] public FallingState fallingState;
        [HideInInspector] public ParachutingState parachutingState;

        #region UmbrellaValues
        
        [HideInInspector] public bool hasUmbrella;

        private bool _canParachute;
        public bool CanParachute
        {
            get => _canParachute;
            set => _canParachute = hasUmbrella && value;
        }

        #endregion

        private BaseState _currentState;

        #endregion

        private void Awake()
        {
            // Set the player controller global reference.
            if (GameManager.Instance.playerController == null)
                GameManager.Instance.playerController = this;
            
            // Get the rigidbody component.
            rb = GetComponent<Rigidbody2D>();
            
            // Get the pie controller component.
            pieController = GetComponent<PieController>();
            
            // Get the input controller component.
            inputController = GetComponent<InputController>();
            
            // Set up the player's states.
            idleState = new IdleState(this);
            walkingState = new WalkingState(this);
            runningState = new RunningState(this);
            crouchingState = new CrouchingState(this);
            jumpingState = new JumpingState(this);
            fallingState = new FallingState(this);
            parachutingState = new ParachutingState(this);
            
            // Set the player's initial state.
            ChangeState(idleState);
        }

        private void Start()
        {
            // Set the player's health.
            CurrentHealth = playerSettings.maxHealth;
        }

        private void Update()
        {
            // Take damage if the player falls too far.
            if (rb.velocity.y < -30.0f && transform.position.y < -30.0f) TakeDamage();
            
            if (GameManager.IsPaused) return;
            
            UpdatePlayerSprite();
            
            _currentState.UpdateState();
            _currentState.HandleInput();
            
            UpdateCoyoteTimeCounter();
            UpdateJumpBufferCounter();
        }

        public void ChangeState(BaseState state)
        {
            _currentState?.ExitState();
            _currentState = state;
            _currentState.EnterState();
        }
        
        public BaseState GetCurrentState() => _currentState;

        /// <summary>
        /// Update the player's sprite based on the direction they are facing.
        /// </summary>
        private void UpdatePlayerSprite()
        {
            var animatorTransform = animator.transform;
            animatorTransform.eulerAngles = inputController.MoveInput.x switch
            {
                // right
                > 0 => Vector3.zero,
                // left
                < 0 => new Vector3(0, 180, 0),
                _ => animatorTransform.eulerAngles
            };
        }
        
        private void UpdateCoyoteTimeCounter() => CoyoteTimeCounter = IsGrounded() ? 
                playerSettings.coyoteTime : CoyoteTimeCounter - Time.deltaTime;
        
        private void UpdateJumpBufferCounter() => JumpBufferCounter = InputManager.Jump.triggered ? 
                playerSettings.jumpBufferTime : JumpBufferCounter - Time.deltaTime;
        
        public bool CanJump() => JumpBufferCounter > 0.0f && CoyoteTimeCounter > 0.0f;
        
        public void ResetCoyoteTimeAndJumpBufferCounter() => CoyoteTimeCounter = JumpBufferCounter = 0.0f;

        public void Bounced(Vector2 force) => rb.AddForce(force);
        
        public void TakeDamage(int damage = 1, Transform enemy = null)
        {
            // Play the hurt sound.
            audioSource.Configure(playerSettings.fartSoundData);
            audioSource.Play();
            
            // Reduce the player's health.
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                LevelUtil.RestartLevel();
            
            // Knock back the player.
            if (enemy != null)
                rb.AddForce((transform.position - enemy.position).normalized * playerSettings.knockbackForce);
        }

        public void DropCurrency(EnemyBase enemy)
        {
            // Get the current currency.
            var currentCurrency = CurrencyManager.Instance.Currency;
            // Calculate the loss based on the enemy's damage percentage.
            var currencyLoss = Mathf.RoundToInt(currentCurrency * (enemy.damagePercentage / 100.0f));
            
            // Calculate the dice drops.
            var diceDrops = CurrencyManager.Instance.CalculateDiceDrops(currencyLoss);
            // Drop the calculated currency.
            foreach (var (coinValue, quantity) in diceDrops)
                CurrencyManager.Instance.DropCurrency(coinValue, quantity, 
                    playerSettings.dropForce, playerSettings.scatterRadius, enemy.transform.position);
            
            // Subtract the currency from the player.
            CurrencyManager.Instance.RemoveCurrency(currencyLoss);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public bool IsGrounded() => !GameManager.IsPaused && Physics2D.OverlapCircle(
            GameObject.FindWithTag("GroundCheck").transform.position, 
            playerSettings.groundCheckRadius, playerSettings.groundLayerMask);
    }
}
