using Managers;

namespace UI.Menus
{
    public class Controls : Menu
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.IsPaused = true;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.IsPaused = false;
        }
    }
}