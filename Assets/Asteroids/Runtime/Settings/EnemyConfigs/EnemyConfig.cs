using System;
using Asteroids.Runtime.Enemies.Mono;
using Asteroids.Runtime.HP.Components;
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
        public int MinDamage;
        public int MaxDamage;
        public int MinHealth;
        public int MaxHealth;
        public int MaxRounds;
        public int ScoreForKill;
        public float ProjectilesSpeed;
        public float Speed;
        public float Acceleration;
        public float Damping;
        public float ShootDelay;
        public float ReloadTime;
        public Damage DamageOnCollision;
    }
}