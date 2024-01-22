using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float _speed;
    private Rigidbody2D _rb;
    private Vector2 _direction = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()//moves the  enemy horizontally
    {
        //_rb.velocity = _direction * _speed;

        gameObject.transform.Translate(_direction * _speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)//turns around when hitting wall
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        { 
         _direction *= new Vector2(-1, 0);
         this.gameObject.transform.localScale *= new Vector2(-1, 1);
        }
        
    }
    protected virtual void Die()//destroys object on death
    {
        Destroy(this.gameObject);
    }


}
