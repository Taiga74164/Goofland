using Controller;
using JetBrains.Annotations;
using Managers;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [CanBeNull] public GameObject rig;
    
    [SerializeField] protected float speed;

    [SerializeField] protected int damage = 1;

    [SerializeField] protected bool useTimer;

    [SerializeField] protected float turnTimer;

    //weaknesses
    [SerializeField] protected bool pieWeakness;
    [SerializeField] protected bool bananaWeakness;
    [SerializeField] protected bool waterWeakness;
    private Vector2 _direction = Vector2.left;
    protected Rigidbody2D rb;

    private float _turnCount;
    
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (rig == null)
            rig = gameObject;
    }

    protected virtual void FixedUpdate()
    {
        if (GameManager.IsPaused) return;
        
        if (useTimer) Timer();

        MoveEnemy();
    }

    private void OnCollisionEnter2D(Collision2D collision) //turns around when hitting wall
    {
        if (collision.IsPlayer())
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(transform,damage);
        else if
            (collision.gameObject.layer !=
             ~LayerMask.NameToLayer(
                 "Player")) //this wont work well depending on how levels are built. will likely have to change
            Turn();
    }

    private void MoveEnemy() //moves the enemy horizontally
    {
        gameObject.transform.Translate(_direction * (speed * Time.deltaTime));
    }

    private void Timer() //swaps direction if x amount of seconds have passed
    {
        _turnCount += Time.deltaTime;
        if (!(_turnCount >= turnTimer)) return;
        Turn();
        _turnCount = 0;
    }

    protected virtual void Die() //destroys object on death
    {
        Destroy(gameObject);
    }

    private void Turn() //changes enemies direction
    {
        _direction *= new Vector2(-1, 0);
        if (_direction == Vector2.left)
        {
            // Rotate the enemy 180 degrees.
            rig!.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (_direction == Vector2.right)
        {
            // Reset the enemy's rotation.
            rig!.transform.eulerAngles = Vector3.zero;
        }
    }

    /// <summary>
    /// Called when the enemy is hit by a weapon.
    /// </summary>
    /// <param name="weapon">The weapon type.</param>
    public virtual void GotHit(IWeapon weapon)
    {
        switch (weapon)
        {
            case Pie when pieWeakness:
                Die();
                break;
            case Pie:
                Debug.Log("wrong type");
                break;
            
            case BananaPeel when bananaWeakness:
                Die();
                break;
            case BananaPeel:
                Debug.Log("wrong type");
                break;
            
            case WaterGunProjectile when waterWeakness:
                Die();
                break;
            case WaterGunProjectile:
                Debug.Log("wrong type");
                break;
        }
    }
}