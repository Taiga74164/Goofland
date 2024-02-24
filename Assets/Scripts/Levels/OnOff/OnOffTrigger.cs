using Objects.Scriptable;
using UnityEngine;

namespace Levels
{
    public class OnOffTrigger : MonoBehaviour, ITrigger
    {
        public GameEvent onTrigger;
        
        [Tooltip("The color that will start active when the scene is loaded.")]  
        [SerializeField] private OnBlock activeColor;

        private SpriteRenderer _sprite; 
        
        public OnBlock ActiveColor 
        { 
            get => activeColor;
            set
            {
                //changes sprite color when block type is changed. likely temporary until art assets are received for it
                activeColor = value;
                _sprite.color = value == OnBlock.Red ? Color.red : Color.blue;
            }
        }

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            ActiveColor = ActiveColor;
        }

        private void Start()
        {
            Trigger();
        }

        public void Trigger() 
        {
            ActiveColor = ActiveColor == OnBlock.Red ? OnBlock.Blue : OnBlock.Red;
            onTrigger.Raise(ActiveColor);
        }

        public void OnTargetEvent(object data)
        {
            if(data is OnBlock blockType)
                ActiveColor = blockType;
        }
    }   

    public enum OnBlock
    {
        Red,
        Blue
    }
}