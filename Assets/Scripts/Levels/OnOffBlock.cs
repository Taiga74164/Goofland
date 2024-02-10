using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Levels
{
    public class OnOffBlock : MonoBehaviour
    {
        //this blocks color
        [SerializeField] private OnBlock _blockType;

        private BoxCollider2D _boxCollider;
        private SpriteRenderer _sprite;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// makes this block active or inactive depending on the color of the target and this objects color.
        /// </summary>
        /// <param name="data"></param>
        public void Switch(object data)
        {
            if(data is OnBlock onType)
            {
                if (onType == _blockType)
                {
                    _boxCollider.enabled = true;
                    //hate this. will change it once we get art for these
                    Color color = _sprite.color;
                    color.a = 1f;
                    _sprite.color = color;
                }
                else
                {
                    _boxCollider.enabled = false;
                    //hate this. will change it once we get art for these
                    Color color = _sprite.color;
                    color.a = .45f;
                    _sprite.color = color;
                    Debug.Log(onType);
                }
            }
        }
    }
}

