using UnityEngine;
using Weapons;

namespace Enemies.Components
{
    public class DurumaDiverHead : MonoBehaviour
    {
        // TODO:
        // Check if hit on the head
        // Ideas:
        // a) Use a separate collider for the head and checking the hit collider.
        // b) Use collider bounds to check if the hit is on the head.
        // c) Use the game object as the head and create child game objects for the stacks.
        public int stacks;

        private void Update()
        {
            // Get child count.
            stacks = transform.childCount;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Pie>())
                ShedStack();
        }

        private void ShedStack()
        {
            if (stacks <= 0) return;
            
            // Destroy the last child.
            Destroy(transform.GetChild(stacks - 1).gameObject);
        }
    }
}