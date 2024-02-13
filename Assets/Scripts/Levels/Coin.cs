using Objects.Scriptable;
using UnityEngine;

namespace Levels
{
    public class Coin : MonoBehaviour
    {
        public GameEvent CoinEvent;
        [SerializeField] private int _value;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("player"))
            {
                CoinEvent.Raise(_value);
                Destroy(gameObject);
            }
                
        }
    }
}


