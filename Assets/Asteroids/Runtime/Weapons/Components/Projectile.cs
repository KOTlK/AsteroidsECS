using Asteroids.Runtime.HP.Components;
using UnityEngine;

namespace Asteroids.Runtime.Weapons.Components
{
    public struct Projectile
    {
        public Damage Damage;
        public int Owner;
        public Vector2 Direction;
        public float Speed;
        public float DistancePassed;
        public float MaxDistance;
    }
}