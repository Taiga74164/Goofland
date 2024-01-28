using System.Collections;
using Managers;
using Objects;
using UnityEngine;

public class WaterGun : MonoBehaviour
{
    public float maxChargeTime = 3.0f;
    public float minDistance = 1.0f;
    public float maxDistance = 3.0f;
    
    public Transform waterSpawnPoint;
    
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
        _chargeTime = Mathf.Min(_chargeTime + Time.deltaTime, maxChargeTime);
    }
    
    public void Shoot()
    {
        if (!gameObject.active) return;
        
        _shotDistance = Mathf.Lerp(minDistance, maxDistance, Mathf.Clamp01(_chargeTime / maxChargeTime));
        
        var projectile = PrefabManager.Create(Prefabs.WaterGunProjectile);
        projectile.transform.SetParent(waterSpawnPoint);
        
        var velocity = new Vector2(_shotDistance, 0.0f);
        var rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
        rb.isKinematic = true;
        
        var duration = _shotDistance / velocity.magnitude;
        StartCoroutine(DestroyAfterDuration(projectile, duration));
        
        // var direction = transform.right;
        // var force = Mathf.Clamp(_chargeTime, 0.0f, maxChargeTime);
        // var hit = Physics2D.Raycast(transform.position, direction, maxDistance);
        
        // if (hit.collider != null)
        // {
        //     var enemy = hit.collider.GetComponent<Enemy>();
        //     if (enemy != null)
        //     {
        //         // TODO: Deal damage to the enemy.
        //     }
        // }
    }
    
    private IEnumerator DestroyAfterDuration(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(obj);
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * maxDistance);
        
        UnityEditor.Handles.Label(transform.position + transform.right * _shotDistance, $"{_shotDistance} units");
    }
    #endif
}
