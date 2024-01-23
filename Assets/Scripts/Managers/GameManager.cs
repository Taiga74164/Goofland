using Controller;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public PlayerController playerController;
        
        private void Start()
        {
            playerController = playerController == null ? FindObjectOfType<PlayerController>() : GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }
}
