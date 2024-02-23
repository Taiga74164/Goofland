using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class Gum : MonoBehaviour
    {
        private Bubble _bubble;
       

        void Awake()
        {
            _bubble = GetComponentInChildren<Bubble>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareLayer("Projectile"))
            {
                _bubble.gameObject.SetActive(true);
            }
        }
    }
}
