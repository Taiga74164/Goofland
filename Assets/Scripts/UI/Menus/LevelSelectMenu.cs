using Utils;

namespace UI.Menus
{
    public class LevelSelectMenu : Menu
    {
        public void OnEnterLevel(string levelName) => LevelUtil.LoadLevel(levelName);
    }
}