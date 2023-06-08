using System;
using UnityEngine;

namespace Asteroids.Runtime.Ships.Components
{
    [Serializable]
    public struct ShipInput
    {
        public Vector2 MovementDirection;
        public Vector2 LookDirection;
    }
}