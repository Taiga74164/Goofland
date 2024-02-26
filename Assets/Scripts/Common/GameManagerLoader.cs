using System;
using UnityEngine;

/// <summary>
/// Static class that loads the game manager into the scene.
/// </summary>
public static class GameManagerLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeGameManager()
    {
        // Add the game manager to the scene.
        var prefab = Resources.Load<GameObject>("Prefabs/Managers/GameManager");
        if (prefab == null) throw new Exception("Missing GameManager prefab!");

        var instance = UnityEngine.Object.Instantiate(prefab);
        if (instance == null) throw new Exception("Failed to instantiate GameManager prefab!");

        instance.name = "Managers.GameManager (Singleton)";
    }
}