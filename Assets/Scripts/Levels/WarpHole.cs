using Controllers;
using UnityEngine;
using System.Collections;

namespace Levels
{
    public class WarpHole : MonoBehaviour
    {
        [SerializeField] private WarpHole otherPortal;
        [SerializeField] private Direction direction;
        [Tooltip("The amount of force added to the player when they exit a portal.")]
        [SerializeField] private float playerExitForce;

        private bool _active = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_active) return;
            
            var rb = collision.GetComponent<Rigidbody2D>();
            if (!rb) return;
            
            otherPortal.Deactivate();
            Debug.Log($"Entered Warp Hole: {rb.velocity}");
            otherPortal.AlterDirection(rb);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Activate();
        }

        public void AlterDirection(Rigidbody2D warpedObject)
        {
           warpedObject.transform.position = transform.position;

            var dir = GetDirectionVector(direction);
            var originalVelocity = warpedObject.velocity.magnitude;

            warpedObject.velocity = dir * originalVelocity;


            if (warpedObject.gameObject.CompareLayer("Player"))
            {
                var playerController = warpedObject.GetComponent<PlayerController>();
                if (direction is Direction.Right || (direction is Direction.Left && !playerController.IsGrounded()))
                {
                    
                    // warpedObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -Physics2D.gravity );
                    warpedObject.GetComponent<PlayerController>().beenWarped = true;
                }
                else if (direction is Direction.Up && !playerController.IsGrounded())
                {
                    warpedObject.AddForce(new Vector2(0, playerExitForce));
                }

                Debug.Log($"Exited Warp Hole: {warpedObject.GetComponent<Rigidbody2D>().velocity}");
                Invoke(nameof(Activate), .5f);
            }
        }

        public void Deactivate()
        {
            _active = false;
           
        }

        private void Activate()
        {
            _active = true;
            
        }
        
        private Vector2 GetDirectionVector(Direction dir)
        {
            return dir switch
            {
                Direction.Left => Vector2.left,
                Direction.Right => Vector2.right,
                Direction.Down => Vector2.down,
                Direction.Up => Vector2.up,
                _ => Vector2.zero,
            };
        }
        private IEnumerator Move(Rigidbody2D warpedObject)
        {
            warpedObject.transform.Translate(transform.position);
            yield return new WaitUntil(() => warpedObject.transform.position == transform.position);
        }

    }


    public enum Direction
    {
        Left = 0,
        Right = 1,
        Down = 2,
        Up = 3
    }
}