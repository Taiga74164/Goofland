using UnityEngine;

namespace Controllers.Components
{
    public class Magnet : MonoBehaviour
    {
        [Tooltip("The radius of the magnet.")]
        [SerializeField] private float magnetRadius = 5.0f;
        [Tooltip("The force of the magnet.")]
        [SerializeField] private float magnetForce = 10.0f;
        [Tooltip("The layer mask of the magnet.")]
        [SerializeField] private LayerMask magnetLayer;
        
        
        private void FixedUpdate()
        {
            var hitColliders = new Collider2D[12];
            var numColliders =
                Physics2D.OverlapCircleNonAlloc(transform.position, magnetRadius, hitColliders, magnetLayer);
            for (var i = 0; i < numColliders; i++)
            {
                var hitCollider = hitColliders[i];
                var rb = hitCollider.GetComponent<Rigidbody2D>();
                var collision = hitCollider.GetComponent<CircleCollider2D>();
                var direction = transform.position - hitCollider.transform.position;
                // SHIT CODE!
                // DO. NOT. LOOK.
                if (Vector2.Distance(transform.position, hitCollider.transform.position) <= 1.0f)
                    collision.isTrigger = true;
                rb!.AddForce(direction.normalized * (magnetForce * Time.fixedDeltaTime), ForceMode2D.Impulse);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, magnetRadius);
        }
    }
}