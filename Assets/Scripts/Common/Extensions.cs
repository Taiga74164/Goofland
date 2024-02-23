using System.Collections.Generic;
using Levels;
using Objects.Scriptable;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Resets the transform's position and rotation.
    /// </summary>
    /// <param name="transform">The transform to reset.</param>
    /// <param name="resetPosition">Whether to reset the position.</param>
    /// <param name="resetRotation">Whether to reset the rotation.</param>
    public static void Reset(this Transform transform,
        bool resetPosition = true,
        bool resetRotation = false)
    {
        if (resetPosition) transform.position = Vector3.zero;

        if (resetRotation) transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Updates the polygon collider shape to match the sprite's shape.
    /// </summary>
    /// <see href="https://discussions.unity.com/t/refreshing-the-polygon-collider-2d-upon-sprite-change/107265/8">
    /// Unity Discussions</see>
    /// <param name="collider">The collider to update.</param>
    /// <param name="sprite">The sprite to use.</param>
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

    /// <summary>
    /// Returns true if the collider is a player.
    /// </summary>
    /// <param name="collider">The collider to check.</param>
    public static bool IsPlayer(this Collider2D collider)
        => collider.CompareTag("Player");

    /// <summary>
    /// Returns true if the collision is a player.
    /// </summary>
    /// <param name="collision">The collision to check.</param>
    public static bool IsPlayer(this Collision2D collision)
        => collision.gameObject.CompareTag("Player");

    /// <summary>
    /// Configures the audio source with the audio data.
    /// </summary>
    /// <param name="audioSource">The audio source to configure.</param>
    /// <param name="audioData">The audio data to use.</param>
    public static void Configure(this AudioSource audioSource, AudioData audioData)
    {
        audioSource.clip = audioData.clip;
        audioSource.outputAudioMixerGroup = audioData.mixerGroup;
        audioSource.playOnAwake = audioData.playOnAwake;
        audioSource.loop = audioData.loop;
    }

    /// <summary>
    /// Converts the currency to a prefab.
    /// </summary>
    /// <param name="currency">The currency to convert.</param>
    /// <returns>The prefab for the currency.</returns>
    public static Prefabs ToCurrencyPrefab(this CoinValue currency)
    {
        return currency switch
        {
            CoinValue.D1 => Prefabs.CoinD1,
            CoinValue.D4 => Prefabs.CoinD4,
            CoinValue.D6 => Prefabs.CoinD6,
            CoinValue.D8 => Prefabs.CoinD8,
            CoinValue.D10 => Prefabs.CoinD10,
            CoinValue.D12 => Prefabs.CoinD12,
            CoinValue.D20 => Prefabs.CoinD20,
            _ => Prefabs.CoinD1
        };
    }

    /// <summary>
    /// Returns true if the game object's layer matches the layer name.
    /// </summary>
    /// <param name="gameObject">The game object to check.</param>
    /// <param name="layerName">The layer name to compare.</param>
    /// <returns>true or false.</returns>
    public static bool CompareLayer(this GameObject gameObject, string layerName)
        => gameObject.layer == LayerMask.NameToLayer(layerName);
}