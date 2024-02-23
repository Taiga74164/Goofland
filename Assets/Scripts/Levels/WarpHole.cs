using Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Levels
{
    public class WarpHole : MonoBehaviour
    {
        [SerializeField] private WarpHole _otherPortal;
        private Vector3[] Directions = new Vector3[] {Vector3.left, Vector3.right, Vector3.down, Vector3.up};
        [SerializeField] private Direction _direction;

        [Tooltip("the amount of force added to the player when they exit a portal")]
        [SerializeField] private float _playerExitForce;

        private bool _active = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Rigidbody2D>() && _active)
            {
                _otherPortal.Deactivate();
                //collision.gameObject.transform.position = _otherPortal.transform.position;
                Debug.Log($"Entered Warp Hole: {collision.GetComponent<Rigidbody2D>().velocity}");
                _otherPortal.AlterDirection(collision.GetComponent<Rigidbody2D>());
                
            }

        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _active = true;
        }

        //dosent work on playr due to player controller handling physics manually 
        public void AlterDirection(Rigidbody2D warpedObject)
        {
            warpedObject.transform.position = transform.position;

            var direction = Directions[(int)_direction].normalized;
            var originalVelocity = warpedObject.velocity.magnitude;

            warpedObject.velocity = direction * originalVelocity;

            //change direction of object coming out of portal
            // warpedObject.velocity = warpedObject.velocity * (warpedObject.velocity.normalized * Directions[(int)_direction]);

            if (warpedObject.gameObject.CompareLayer("Player"))
            {
                if(_direction == Direction.left || _direction == Direction.right)
                {
                    warpedObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -Physics2D.gravity );
                    warpedObject.GetComponent<PlayerController>().beenWarped = true;
                }

               
                Debug.Log($"exit Warp Hole: {warpedObject.GetComponent<Rigidbody2D>().velocity}");
            }
 
        }

        public void Deactivate()
        {
            _active = false;
        }

    }
    public enum Direction
    {
        left = 0,
        right = 1,
        down = 2,
        up = 3
    }

}


