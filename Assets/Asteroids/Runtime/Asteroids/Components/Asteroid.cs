using Asteroids.Runtime.HP.Components;
using UnityEngine;

namespace Asteroids.Runtime.Asteroids.Components
{
    public struct Asteroid
    {
        public Damage Damage;
        public Vector2 MovementDirection;
        public int Reward;
        public float Speed;
        public float LifeTime;
    }
}