namespace Asteroids.Runtime.UI
{
    public class PlayerShipSelector : Window
    {
        private PlayerShipReference[] _references;
        
        public PlayerShipReference Selected { get; private set; }
        
        private void Awake()
        {
            _references = GetComponentsInChildren<PlayerShipReference>();
            Selected = _references[0];
            _references[0].Highlight();

            foreach (var reference in _references)
            {
                reference.Clicked += SelectAndHighlight;
            }
        }

        private void OnDestroy()
        {
            foreach (var reference in _references)
            {
                reference.Clicked -= SelectAndHighlight;
            }
        }

        private void SelectAndHighlight(PlayerShipReference shipReference)
        {
            Selected.RemoveHighlight();
            Selected = shipReference;
            Selected.Highlight();
        }
    }
}