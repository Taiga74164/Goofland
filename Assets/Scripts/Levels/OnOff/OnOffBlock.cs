using UnityEngine;

namespace Levels
{
    public class OnOffBlock : MonoBehaviour
    {
        [Tooltip("this blocks color")]
        [SerializeField] private OnBlock blockType;
        [SerializeField] private LayerMask playerLayer;
        
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
            if (data is OnBlock onType)
            {
                if (onType == blockType)
                {
                    _boxCollider.excludeLayers |= 1 << playerLayer;
                    //this will be changed once we have assets for blocks being on and off
                    Color color = _sprite.color;
                    color.a = 1f;
                    _sprite.color = color;
                }
                else
                {
                    _boxCollider.excludeLayers &= ~(1 << playerLayer);
                    //this will be changed once we have assets for blocks being on and off
                    Color color = _sprite.color;
                    color.a = .45f;
                    _sprite.color = color;
                }
            }
        }
    }
}

