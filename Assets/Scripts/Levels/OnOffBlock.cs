using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class OnOffBlock : MonoBehaviour
    {
        //this blocks color
        [SerializeField] private OnBlock _blockType;

        private BoxCollider2D _boxCollider;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        public void Switch(object data)
        {
            if(data is OnBlock onType)
            {
                if (onType == _blockType)
                {
                    _boxCollider.enabled = true;
                }
                else
                {
                    _boxCollider.enabled = false;
                    Debug.Log(onType);
                }
            }
        }
    }
}

