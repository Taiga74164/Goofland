using Managers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pie : MonoBehaviour, IWeapon
{
    public Vector2 direction = new Vector2(1, 3);
    public float throwForce = 2.0f;
    
    private Rigidbody2D _rigidbody2D;

    [Header("Debugging")]
    public bool debug;
    private Vector3 _initialPosition;
    private float _travelDistance;

    [Header("Audio")]
    public GameEvent onImpact;
    [SerializeField] private AudioSource audioSource;

    private readonly float _spawnOffSet = 0.1f;
    
    private void Awake()
    {
        this.GetComponent<Transform>().Translate(new Vector3(0, _spawnOffSet, 0)); //prevents pie from being destroyed on spawn
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.isKinematic = true;
    }

    private void LateUpdate()
    {
        if (GameManager.IsPaused) return;
        
        if (transform.position.y < -100)
            Destroy(gameObject);
        
        #region Debugging

        if (debug)
            _travelDistance = Vector3.Distance(_initialPosition, transform.position);

        #endregion
    }

    public void ThrowPie(Vector2 playerVelocity)
    {
        var forceDirection = direction.normalized;
        _rigidbody2D.isKinematic = false;
        var force = forceDirection * throwForce + (playerVelocity / 2);
        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);

        #region Debugging

        if (debug)
            _initialPosition = transform.position;

        #endregion
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            onImpact.Raise(audioSource);
            // TODO: Deal damage to the enemy.
            other.gameObject.GetComponent<Enemy>().GotHit(this);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Invoke(nameof(Die), audioSource.clip.length);
            //Destroy(gameObject);
        }
        else if(!other.IsPlayer())
        {
            onImpact.Raise(audioSource);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            Invoke(nameof(Die), audioSource.clip.length);
            //Destroy(gameObject);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_initialPosition, transform.position);
    }
#endif
    
    public bool Enabled
    {
        get => this.enabled;
        set => this.enabled = value;
    }
}
