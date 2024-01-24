using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class BananaPeel : MonoBehaviour, IWeapon
{
    public float explosionRadius = 3.0f;
    public bool isPlaced;
    
    private CircleCollider2D _circleCollider2D;
    private Rigidbody2D _rigidbody2D;
    
    private void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _circleCollider2D.isTrigger = true;
        
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.isKinematic = true;
        
        PlaceMine();
    }

    private void PlaceMine()
    {
        isPlaced = true;
    }

    public void Explode()
    {
        isPlaced = false;
        
        // Get all colliders within the explosion radius.
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.gameObject.GetComponent<Enemy>().GotHit(this);
            }
        }
        
        // Particle Effects
        
        // Sound Effects
        
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }      
    }
}
