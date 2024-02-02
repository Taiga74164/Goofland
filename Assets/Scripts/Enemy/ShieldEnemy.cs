using UnityEngine;

public class ShieldEnemy : Enemy
{
    [SerializeField] private int health = 2;

    protected override void Die() //allows enemy to take an additional hit before dying
    {
        health--;
        if(health <= 0)
        {
            base.Die();
        }
        else
        {
            Debug.Log("still alive");
        }
    }
}
