using Managers;
using Objects.Scriptable;
using UnityEngine;
using Weapons;

namespace Levels
{
    public class Balloon : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.GetComponent<Pie>()) return;
            
            var piano = PrefabManager.Create<Piano>(Prefabs.Piano);
            piano.transform.position = transform.position;
            piano.DropPiano();
            Destroy(gameObject);
        }
    }
}