using System;

namespace Asteroids.Runtime.UI
{
    [Serializable]
    public class ViewRoot
    {
        public MainMenu MainMenu;
        public InGameWindow InGameWindow;
        public LoseWindow LoseWindow;
        public PauseWindow PauseWindow;
    }
}