using System.Linq;
using Controller;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public PlayerController playerController;

        private bool _isPaused;
        public static bool IsPaused
        {
            get => Instance._isPaused;
            set => Instance._isPaused = value;
            // Instance.playerController.enabled = !value;
            // FindObjectOfType<PieController>().enabled = !value;
            // FindObjectsOfType<MonoBehaviour>().OfType<IWeapon>().ToList().ForEach(
            //     w => w.Enabled = !value);
            // FindObjectsOfType<Enemy>().ToList().ForEach(e => e.enabled = !value);
        }
    }
}
