using System;
using UnityEngine;

namespace Asteroids.Runtime.Input.Components
{
    [Serializable]
    public class InputMap
    {
        public KeyCode Shoot = KeyCode.Space;
        public KeyCode Reload = KeyCode.R;
        public KeyCode PauseGame = KeyCode.Escape;
    }
}