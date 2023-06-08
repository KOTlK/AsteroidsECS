using System;
using UnityEngine;

namespace Asteroids.Runtime.Ships.Components
{
    [Serializable]
    public struct Ship
    {
        public float MaxSpeed;
        public float Acceleration;
        public float Damping;
        public float RotationSpeed;
        public Vector2 Velocity;
    }
}