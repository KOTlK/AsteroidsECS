using System;
using Asteroids.Runtime.Enemies.Components;
using Asteroids.Runtime.Enemies.Mono;
using Asteroids.Runtime.Weapons.Mono;
using UnityEngine;

namespace Asteroids.Runtime.Settings.EnemyConfigs
{
    [Serializable]
    [CreateAssetMenu(menuName = "New/Enemy", fileName = "Enemy", order = 1)]
    public class EnemyConfig : ScriptableObject
    {
        public EnemyView Prefab;
        public ProjectileView ProjectilePrefab;
        public Vector2 Size;
        public Vector2 ProjectileColliderSize;
        public Enemy Enemy;
        public int MinDamage;
        public int MaxDamage;
        public int MinHealth;
        public int MaxHealth;
        public int MaxRounds;
        public float ProjectilesSpeed;
        public float Speed;
        public float Acceleration;
        public float Damping;
        public float RotationSpeed;
        public float ShootDelay;
        public float ShootRange;
        public float ReloadTime;
    }
}