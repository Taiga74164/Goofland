﻿using Managers;
using UnityEngine;

namespace Enemies.Projectile
{
    public class MusicNoteProjectile : MonoBehaviour
    {
        public Vector2 direction = Vector2.left;
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private float maxDistance = 5.0f;
        
        private Rigidbody2D _rb;
        private Vector2 _startPosition;

        private void Start()
        {
            // Get the rigidbody component.
            _rb = GetComponent<Rigidbody2D>();
            
            // Set the rigidbody to kinematic.
            _rb.isKinematic = true;
            
            // Set the start position.
            _startPosition = transform.position;
            
            // Set the velocity of the projectile.
            _rb.velocity = direction * speed;
        }

        private void Update()
        {
            // If the projectile has traveled the maximum distance, destroy it.
            if (Vector2.Distance(_startPosition, transform.position) >= maxDistance)
                Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Dirty code.
            if (other.CompareTag("Enemy")) return;
            
            if (other.CompareTag("GroundCheck"))
            {
                // FYI: Please update to Unity 2022.
                // other.transform.parent.GetComponent<PlayerController>().TakeDamage();
                GameManager.Instance.playerController.TakeDamage();
            }
            
            Destroy(gameObject);
        }
    }
}