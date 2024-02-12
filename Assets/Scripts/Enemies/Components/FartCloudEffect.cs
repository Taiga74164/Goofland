﻿using Controller;
using UnityEngine;

namespace Enemies.Components
{
    public class FartCloudEffect : MonoBehaviour
    {
        public float fartArea;
        private CircleCollider2D _circleCollider2D;

        private void Start()
        {
            // Get the circle collider 2D component.
            _circleCollider2D = GetComponent<CircleCollider2D>();
            
            // Set the radius of the circle collider 2D.
            _circleCollider2D.radius = fartArea;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.IsPlayer())
                other.GetComponent<PlayerController>().TakeDamage();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, fartArea);
        }
#endif
    }
}