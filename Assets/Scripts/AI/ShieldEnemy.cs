using UnityEngine;
public class ShieldEnemy : Enemy
{
    [SerializeField] private int _health = 2;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Die() //allows enemy to take an additional hit before dieing
    {
        _health--;
        if(_health <= 0)
        {
            base.Die();
        }
        else
        {
            Debug.Log("still alive");
        }
        
    }
}
