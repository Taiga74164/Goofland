using UnityEngine;

public class WaterGun : MonoBehaviour
{
    public float maxChargeTime = 3.0f;
    public float maxDistance = 3.0f;

    private float _chargeTime;
    private float _shotDistance;

    private void Update()
    {
        if (!gameObject.active) return;
        Debug.Log($"_chargeTime: {_chargeTime}, _shotDistance: {_shotDistance}");;
    }

    public void Charge()
    {
        if (!gameObject.active) return;
        _chargeTime = 0.0f;
    }
    
    public void Charging()
    {
        if (!gameObject.active) return;
        _chargeTime += Time.deltaTime;
    }
    
    public void Shoot()
    {
        if (!gameObject.active) return;
        
        _shotDistance = Mathf.Lerp(maxDistance, 1, Mathf.Clamp01(_chargeTime / maxChargeTime));
        
        var direction = transform.right;
        var force = Mathf.Clamp(_chargeTime, 0.0f, maxChargeTime);
        var hit = Physics2D.Raycast(transform.position, direction, maxDistance);
        
        
        // if (hit.collider != null)
        // {
        //     var enemy = hit.collider.GetComponent<Enemy>();
        //     if (enemy != null)
        //     {
        //         // TODO: Deal damage to the enemy.
        //     }
        // }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * maxDistance);
        
        UnityEditor.Handles.Label(transform.position + transform.right * _shotDistance, $"{_shotDistance} units");
    }
}
