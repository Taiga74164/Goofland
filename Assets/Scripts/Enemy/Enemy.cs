using Controller;
using JetBrains.Annotations;
using Managers;
using UnityEngine;
using Weapons;

/// <summary>
/// Defines the base class for all enemies in the game.
/// </summary>
public class Enemy : MonoBehaviour
{
    [CanBeNull] public GameObject model;
    
    [SerializeField] protected float speed;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected bool useTimer;
    [SerializeField] protected float turnTimer;

    [Header("Weaknesses")]
    [SerializeField] protected bool pieWeakness;
    [SerializeField] protected bool bananaWeakness;
    [SerializeField] protected bool waterWeakness;
    
    private Vector2 _direction = Vector2.left;
    private float _turnCount;
    
    protected virtual void Start()
    {
        model = model ? model : gameObject;
    }

    protected virtual void FixedUpdate()
    {
        if (GameManager.IsPaused) return;
        
        if (useTimer) Timer();

        MoveEnemy();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.IsPlayer())
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(damage);
            player.KnockBack(transform);
        }
        else if (collision.gameObject.layer != ~LayerMask.NameToLayer("Player"))
        {
            Turn();
        }
    }

    private void MoveEnemy() => transform.Translate(_direction * (speed * Time.deltaTime));

    /// <summary>
    /// Moves the enemy in the opposite direction after a certain amount of time.
    /// </summary>
    private void Timer()
    {
        _turnCount += Time.deltaTime;
        if (!(_turnCount >= turnTimer)) return;
        Turn();
        _turnCount = 0;
    }

    protected virtual void Die() => Destroy(gameObject);

    private void Turn()
    {
        _direction *= new Vector2(-1, 0);
        if (_direction == Vector2.left)
        {
            // Rotate the enemy 180 degrees.
            model!.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (_direction == Vector2.right)
        {
            // Reset the enemy's rotation.
            model!.transform.eulerAngles = Vector3.zero;
        }
    }

    /// <summary>
    /// Called when the enemy is hit by a weapon.
    /// </summary>
    /// <param name="weapon">The weapon type.</param>
    public virtual void GotHit(IWeapon weapon)
    {
        var isWeak = (weapon is Pie && pieWeakness) || 
                     (weapon is BananaPeel && bananaWeakness) ||
                     (weapon is WaterGunProjectile && waterWeakness);
        
        if (isWeak) Die();
    }

    public enum EnemyState
    {
        Colorless,
        Color,
    }
}