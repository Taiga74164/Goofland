using System.Collections;
using System.Collections.Generic;
using Objects.Scriptable;
using UnityEngine;

namespace Levels
{
    public class OnOffTrigger : MonoBehaviour, ITrigger
    {
        public GameEvent onTrigger;
        
        // The color that will start active when the scene is loaded  
        [SerializeField] private OnBlock _activeColor;

        private SpriteRenderer _sprite; 
        
        public OnBlock ActiveColor 
        { 
            get 
            { 
                return _activeColor; 
            } 
            set //changes sprite color when block type is changed. likely temporary until art assets are recieved for it
            { 
                _activeColor = value;
                if(value == OnBlock.Red)
                {
                    _sprite.color = Color.red;
                }
                else
                {
                    _sprite.color = Color.blue;
                }
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
            if(ActiveColor == OnBlock.Red)
            {
                ActiveColor = OnBlock.Blue;
            }
            else
            {
                ActiveColor = OnBlock.Red;
            }
            onTrigger.Raise(ActiveColor);
        }

        public void OnTargetEvent(object data)
        {
            if(data is OnBlock blockType)
                ActiveColor = blockType;
        }
    }   

    //block colors
    public enum OnBlock
    {
        Red,
        Blue
    }
}

