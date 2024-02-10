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

        private void Start()
        {
            Trigger();
        }

        public void Trigger() 
        {
            if(_activeColor == OnBlock.Red)
            {
                _activeColor = OnBlock.Blue;
            }
            else
            {
                _activeColor = OnBlock.Red;
            }
            onTrigger.Raise(_activeColor);
        }
    }   

    //block colors
    public enum OnBlock
    {
        Red,
        Blue
    }
}

