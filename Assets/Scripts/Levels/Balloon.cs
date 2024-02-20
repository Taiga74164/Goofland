using Managers;
using Objects.Scriptable;
using UnityEngine;
using Weapons;

namespace Levels
{
    public class Balloon : MonoBehaviour
    {
        [SerializeField] private bool _respawning;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.GetComponent<Pie>()) return;
            var piano = PrefabManager.Create<Piano>(Prefabs.Piano);
            piano.transform.position = transform.position;
            piano.DropPiano();

            if (!_respawning)
                Destroy(gameObject);
            else
            {
                piano.transform.SetParent(gameObject.transform);
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;
            }
        }

        public void Respawn()
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}