using UnityEngine;

namespace Enemies
{
    public class DurumaDiver : Enemy
    {
        [SerializeField] private int health = 2;

        protected override void Die()
        {
            health--;
            if(health <= 0)
            {
                base.Die();
            }
            else
            {
                Debug.Log("still alive");
            }
        }
    }
}
