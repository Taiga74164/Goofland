using UnityEngine;
using UnityEngine.SceneManagement;

namespace Levels
{
    public class EndGame : MonoBehaviour, ITrigger
    {
        public GameEvent OnComplete;
        public void Trigger()
        {
            Debug.Log("end game");
            OnComplete.Raise(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
