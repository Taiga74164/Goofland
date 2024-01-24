using Controller;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed;

    [SerializeField] protected int damage = 1;

    [SerializeField] protected bool useTimer;

    [SerializeField] protected float turnTimer;

    //weaknesses
    [SerializeField] protected bool pieWeakness;
    [SerializeField] protected bool bananaWeakness;
    [SerializeField] protected bool waterWeakness;
    private Vector2 _direction = Vector2.right;
    private Rigidbody2D _rb;

    private float _turnCount;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (useTimer) Timer();

        MoveEnemy();
    }

    private void OnCollisionEnter2D(Collision2D collision) //turns around when hitting wall
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        else if
            (collision.gameObject.layer !=
             LayerMask.NameToLayer(
                 "Ground")) //this wont work well depending on how levels are built. will likely have to change
            Turn();
    }

    private void MoveEnemy() //moves the  enemy horizontally
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
        gameObject.transform.localScale *= new Vector2(-1, 1);
    }

    /// <summary>
    /// Called when the enemy is hit by a weapon.
    /// </summary>
    /// <param name="weapon">The weapon type.</param>
    public void GotHit(IWeapon weapon)
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