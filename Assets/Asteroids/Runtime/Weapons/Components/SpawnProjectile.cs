using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Weapons.Mono;
using UnityEngine;

namespace Asteroids.Runtime.Weapons.Components
{
    public struct SpawnProjectile
    {
        public ProjectileView Prefab;
        public int Owner;
        public float Speed;
        public float Distance;
        public Damage Damage;
        public Vector2 Size;
        public Vector2 Position;
        public Vector2 Direction;
        public PhysicsLayer Targets;
    }
}