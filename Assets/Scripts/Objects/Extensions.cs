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
}
