using Controller;
using Managers;
using UnityEngine;

namespace Enemies
{
    public class Umbregglla : Enemy
    {
        /*
         * An eggy with an umbrella hat falls from the top of the screen to the bottom of the screen.
         * They mostly just act as falling hazards. This enemy randomly spawns from the skies but we will have places where they can’t spawn.
         * It would help if there was a spawner block that the level designers could place and a despawner block.
         */
        [Header("Umbregglla Settings")]
        [SerializeField] private float fallSpeed = 5.0f;
        
        protected override void Update()
        {
            if (GameManager.IsPaused) return;
            
            // Turn towards the player.
            Turn();
            
            // Move the enemy.
            transform.Translate(Vector2.down * (fallSpeed * Time.deltaTime));
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.IsPlayer())
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage();
                Die();
            } 
            else if (collision.gameObject.layer != ~LayerMask.NameToLayer("Player"))
            {
                Die();
            }
        }

        protected override void MoveEnemy()
        {
        }
        
        protected override void Turn()
        {
            // Get the player's position.
            var playerPosition = GameManager.Instance.playerController.transform.position;
            // Determine the direction to face.
            var playerIsRight = playerPosition.x > transform.position.x;
            
            // Set the direction and model rotation.
            direction = playerIsRight ? Vector2.right : Vector2.left;
            model!.transform.eulerAngles = playerIsRight ? Vector3.zero : new Vector3(0, 180, 0);
        }
    }
}