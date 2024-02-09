using UnityEngine;

namespace Controller.States
{
    /// <summary>
    /// Abstract class for player states.
    /// </summary>
    public abstract class PlayerState
    {
        public string name;
        protected PlayerController player;
        protected InputController input;
        protected PlayerState(string name, PlayerController player)
        {
            this.name = name;
            this.player = player;
            input = player.inputController;
        }
        
        #region Cached Properties

        protected static readonly int Idle = Animator.StringToHash("Idle");
        protected static readonly int Walking = Animator.StringToHash("Walking");
        protected static readonly int Running = Animator.StringToHash("Running");
        protected static readonly int Jumping = Animator.StringToHash("Jumping");
        protected static readonly int Falling = Animator.StringToHash("Falling");
        protected static readonly int Attacking = Animator.StringToHash("Attacking");

        #endregion
        
        public virtual void EnterState() { }

        public virtual void HandleInput() { }

        public virtual void UpdateState() { }
        
        public virtual void ExitState() { }
    }
}
