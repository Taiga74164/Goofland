using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float _speed;

    [SerializeField] protected int _damage = 1;

    [SerializeField] protected bool _useTimer;
    [SerializeField] protected float _turnTimer;
    //weaknesses
    [SerializeField] protected bool _pieWeakness;
    [SerializeField] protected bool _bananaWeakness;
    [SerializeField] protected bool _waterWeakness;

    private float _turnCount = 0;
    private Rigidbody2D _rb;
    private Vector2 _direction = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    { 
        
        if(_useTimer)
        {
            Timer();
        }

        MoveEnemy();
    }

    private void MoveEnemy()//moves the  enemy horizontally
    {  
        gameObject.transform.Translate(_direction * _speed * Time.deltaTime);
    }

    private void Timer() //swaps direction if x amount of seconds have passed
    {
        _turnCount += Time.deltaTime; 
        if (_turnCount >= _turnTimer)
        {
            Turn();
            _turnCount = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)//turns around when hitting wall
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Controller.PlayerController>().TakeDamage(_damage);
        }
        else if(collision.gameObject.layer != LayerMask.NameToLayer("Ground")) //this wont work well depending on how levels are built. will likely have to change
        {
            Turn();
        }
        
    }
    protected virtual void Die()//destroys object on death
    {
        Destroy(this.gameObject);
    }

    private void Turn() //changes enemies direction
    {
        _direction *= new Vector2(-1, 0);
        this.gameObject.transform.localScale *= new Vector2(-1, 1);
    }

    public virtual void GotHit(GameObject attack) //checks if the projectile that hit the enemy is able to hurt it
    {
        if(attack.GetComponent<Pie>() != null)
        {
            if(_pieWeakness)
            {
                Die();
            }
            else
            {
                Debug.Log("wrong type");
            }
        }
        else if(attack.GetComponent<BananaPeel>() != null)
        {
            if (_bananaWeakness)
            {
                Die();
            }
            else
            {
                Debug.Log("wrong type");
            }
        }
        else if(attack.GetComponent<WaterGunProjectile>() != null)
        {
            if (_waterWeakness)
            {
                Die();
            }
            else
            {
                Debug.Log("wrong type");
            }
        }
        
    }


}
