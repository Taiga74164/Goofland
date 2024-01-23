using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pie : MonoBehaviour
{
    public Vector2 direction = new Vector2(1, 3);
    public float throwForce = 2.0f;
    
    private Rigidbody2D _rigidbody2D;

    private float _spawnOffSet = .1f;
    private void Start()
    {
        this.GetComponent<Transform>().Translate(new Vector3(0, _spawnOffSet, 0)); //prevents pie from being destroyed on spawn
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.isKinematic = true;
        ThrowPie();
    }

    private void LateUpdate()
    {
        if (transform.position.y < -100)
            Destroy(gameObject);
    }

    public void ThrowPie()
    {
        var forceDirection = direction.normalized;
        _rigidbody2D.isKinematic = false;
        _rigidbody2D.AddForce(forceDirection * throwForce, ForceMode2D.Impulse);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // TODO: Deal damage to the enemy.
            other.gameObject.GetComponent<Enemy>().GotHit(gameObject);
            Destroy(gameObject);
        }
        else if(!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
