using Managers;
using Objects.Scriptable;
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
        public int stacks = 3;

        private void Start()
        {
            GenerateStack(stacks);
        }
        
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

        private void GenerateStack(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var stack = PrefabManager.Create(Prefabs.DurumaDiverStack, transform);
                var stackHeight = stack.GetComponent<SpriteRenderer>().bounds.size.y;
                
                // Set the position of the stack.
                stack.transform.localPosition = new Vector3(0, -stackHeight * (i + 1), 0);
                stack.name = $"Stack {i + 1}";
            }
        }

        private void ShedStack()
        {
            if (stacks <= 0) return;
            
            // Destroy the last child.
            var lastChild = transform.GetChild(stacks - 1);
            Destroy(lastChild.gameObject);
        }
    }
}