using System;
using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Weapons.Mono;
using UnityEngine;

namespace Asteroids.Runtime.Weapons.Components
{
    [Serializable]
    public struct Weapon
    {
        public Damage Damage;
        public PhysicsLayer Targets;
        public float TransformOffset;
        public float ShootDelay;
        public float ReloadTime;
        public float ProjectileSpeed;
        public float ProjectileDistance;
        public Vector2 ProjectileSize;
        public int RoundsLeft;
        public int RoundsMax;
        public ProjectileView ProjectilePrefab;
    }
}