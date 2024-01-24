using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Resets the transform's position and rotation.
    /// </summary>
    public static void Reset(this Transform transform,
        bool resetPosition = true,
        bool resetRotation = false)
    {
        if (resetPosition)
        {
            transform.position = Vector3.zero;
        }

        if (resetRotation)
        {
            transform.rotation = Quaternion.identity;
        }
    }
    
    /// <summary>
    /// Updates the polygon collider shape to match the sprite's shape.
    /// </summary>
    /// <see href="https://discussions.unity.com/t/refreshing-the-polygon-collider-2d-upon-sprite-change/107265/8">
    /// Unity Discussions</see>
    public static void UpdateShapeToSprite(this PolygonCollider2D collider, Sprite sprite)
    {
        // Ensure both valid.
        if (collider == null || sprite == null) return;
        // Update count.
        collider.pathCount = sprite.GetPhysicsShapeCount();
                
        // New paths variable.
        var path = new List<Vector2>();

        // Loop path count.
        for (var i = 0; i < collider.pathCount; i++)
        {
            // Clear.
            path.Clear();
            // Get shape.
            sprite.GetPhysicsShape(i, path);
            // Set path.
            collider.SetPath(i, path.ToArray());
        }
    }
}
