using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float _speed;
    [SerializeField] protected float _turnTimer;
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
        
        _turnCount += Time.deltaTime; //timer
        if( _turnCount >= _turnTimer )
        {
            Turn();
            _turnCount = 0;
        }

        MoveEnemy();
    }

    private void MoveEnemy()//moves the  enemy horizontally
    {  
        gameObject.transform.Translate(_direction * _speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)//turns around when hitting wall
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Ground")) //this wont work well depending on how levels are built. will likely have to change
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


}
