using Controllers;
using UnityEngine;

public class Umbrella : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.IsPlayer()) return;
        
        collision.GetComponent<PlayerController>().hasUmbrella = true;
        Destroy(gameObject);
    }
}
