using Managers;
using UnityEngine.InputSystem;

public abstract class Menu<T> : Singleton<T> where T : Menu<T>
{
    public void Open() => gameObject.SetActive(true);

    public void Close() => gameObject.SetActive(false);
}

public abstract class Menu : Menu<Menu>
{
    protected virtual void Update()
    {
        if (InputManager.Return.WasReleasedThisFrame())
            CloseMenu();
    }
    
    public bool IsOpen => MenuManager.Instance.IsMenuOpen(this);
    
    public void OpenMenu() => MenuManager.Instance.OpenMenu(this);
    
    public void CloseMenu() => MenuManager.Instance.CloseMenu();
    
    public virtual void OnBackPressed() => CloseMenu();
}