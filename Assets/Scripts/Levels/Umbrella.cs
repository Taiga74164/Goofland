using Controllers;
using UnityEngine;

public class Umbrella : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<PlayerController>()) return;
        
        other.GetComponent<PlayerController>().hasUmbrella = true;
        Destroy(gameObject);
    }
}
