using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Levels
{
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Platform Settings")]
        public List<Transform> waypoints;
        public float speed = 1.0f;
        
        [Header("Platform State")]
        public List<Rigidbody2D> playersOnPlatform = new List<Rigidbody2D>();
        public Vector3 lastPosition;
        private Transform _transform;

        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private Vector3 _destination;
        
        private Rigidbody2D _rb;
        private BoxCollider2D _collider;
        
        private int _targetIndex;
        
        private void Start()
        {
            // Get the rigidbody component.
            _rb = GetComponent<Rigidbody2D>();
            _rb.isKinematic = true;
            
            // Get the collider component.
            _collider = GetComponent<BoxCollider2D>();

            // Set the transform component and last position.
            _transform = transform;
            lastPosition = _transform.position;
        }

        private void Update()
        {
            if (GameManager.IsPaused) return;
            
            transform.position = Vector3.MoveTowards(transform.position, 
                waypoints[_targetIndex].position, 
                speed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (GameManager.IsPaused) return;
            
            if (transform.position != waypoints[_targetIndex].position) return;
            _targetIndex = _targetIndex == waypoints.Count - 1 ? 0 : _targetIndex + 1;
        }
        
        private void LateUpdate()
        {
            if (GameManager.IsPaused) return;
            
            foreach (var playerRb in playersOnPlatform)
            {
                if (playerRb == null) continue;
                
                var velocity = _transform.position - lastPosition;
                playerRb.transform.Translate(velocity, _transform);
            }

            lastPosition = _transform.position;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.IsPlayer()) return;
            
            var playerRb = other.gameObject.GetComponent<Rigidbody2D>();
                
            var top = _collider.bounds.max.y;
            var bottom = other.collider.bounds.min.y;

            if (!(bottom > top)) return;
            
            AddPlayerToPlatform(playerRb);
                
            Debug.Log("Player is on platform.");
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (!other.IsPlayer()) return;
            
            var playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            RemovePlayerFromPlatform(playerRb);
        }
        
        private void AddPlayerToPlatform(Rigidbody2D playerRb)
        {
            if (playersOnPlatform.Contains(playerRb)) return;
            playersOnPlatform.Add(playerRb);
        }
        
        private void RemovePlayerFromPlatform(Rigidbody2D playerRb)
        {
            if (!playersOnPlatform.Contains(playerRb)) return;
            playersOnPlatform.Remove(playerRb);
        }
    }
}