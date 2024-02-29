using UnityEngine;

namespace Levels
{
    public class OnOffBlock : MonoBehaviour
    {
        [Tooltip("this blocks color")]
        [SerializeField] private OnBlock blockType;
        
        private BoxCollider2D _boxCollider;
        private SpriteRenderer _sprite;
        [SerializeField]  private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;

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
                    // Include the player layer.
                    _boxCollider.excludeLayers &= ~(1 << LayerMask.NameToLayer("Player"));
                    
                    //this will be changed once we have assets for blocks being on and off
                    _sprite.sprite = _onSprite;
                }
                else
                {
                    // Exclude the player layer.
                    _boxCollider.excludeLayers |= 1 << LayerMask.NameToLayer("Player");
                    //this will be changed once we have assets for blocks being on and off
                    _sprite.sprite = _offSprite;
                }
            }
        }
    }
}

