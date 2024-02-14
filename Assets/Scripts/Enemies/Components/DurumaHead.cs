using Controller;
using Managers;
using Objects.Scriptable;
using UnityEngine;
using Weapons;

namespace Enemies.Components
{
    public class DurumaHead : MonoBehaviour
    {
        public int stacks = 3;

        private void Start() => GenerateStack(stacks);
        
        private void Update() => stacks = transform.childCount;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.IsPlayer())
                other.gameObject.GetComponent<PlayerController>().TakeDamage(enemy: transform);
            else if (other.gameObject.GetComponent<Pie>())
                ShedStack();
        }

        private void GenerateStack(int count)
        {
            float totalHeight = 0;
            for (var i = 0; i < count; i++)
            {
                var stack = PrefabManager.Create(Prefabs.DurumaDiverStack, transform);
                var stackHeight = stack.GetComponent<SpriteRenderer>().bounds.size.y;

                // Subtract the height of the new stack from the total height.
                totalHeight -= stackHeight;

                // Set the position of the stack.
                stack.transform.localPosition = new Vector3(0, totalHeight, 0);
                stack.name = $"Stack {i + 1}";

                // Add the height of the new stack to the total height.
                totalHeight += stackHeight;
            }

            // Set the position of the DurumaHead to the top of the stacks.
            transform.position = new Vector3(transform.position.x, -totalHeight, transform.position.z);
        }

        private void ShedStack()
        {
            if (stacks <= 0) return;
            
            // Destroy the last child.
            var lastChild = transform.GetChild(stacks - 1);
            var stack = lastChild.GetComponent<DurumaStack>();
            stack.Die();
        }
    }
}