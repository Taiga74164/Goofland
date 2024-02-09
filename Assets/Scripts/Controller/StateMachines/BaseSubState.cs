namespace Controller.StateMachines
{
    /// <summary>
    /// Sub state class for player states.
    /// </summary>
    public abstract class BaseSubState
    {
        public string name;
        protected BaseState parentState;
        protected PlayerController player;
        protected InputController input;
        protected BaseSubState(string name, BaseState parentState)
        {
            this.name = name;
            this.parentState = parentState;
            this.player = parentState.player;
            input = player.inputController;
        }
        
        public virtual void EnterSubState() { }
        
        public virtual void HandleInput() { }
        
        public virtual void UpdateSubState() { }
        
        public virtual void ExitSubState() { }
    }
}
