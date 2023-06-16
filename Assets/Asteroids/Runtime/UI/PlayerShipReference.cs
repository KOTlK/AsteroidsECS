using System;
using Asteroids.Runtime.Settings.PlayerConfigs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asteroids.Runtime.UI
{
    public class PlayerShipReference : Window, IPointerClickHandler
    {
        public event Action<PlayerShipReference> Clicked = delegate {  };

        [SerializeField] private Outline _outline;
        
        public PlayerConfig Config;

        public void Highlight()
        {
            _outline.enabled = true;
        }

        public void RemoveHighlight()
        {
            _outline.enabled = false;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked.Invoke(this);
        }
    }
}