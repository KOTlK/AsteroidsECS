using Asteroids.Runtime.AI.Components;
using Asteroids.Runtime.Application;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.Enemies.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Input.Components;
using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.Transforms.Components;
using Asteroids.Runtime.Utils.Components;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Random = System.Random;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Enemies.Systems
{
    public class EnemiesSpawnSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Ship, Enemy>> _shipsFilter = default;
        private readonly EcsWorldInject _world = default;
        private readonly EcsCustomInject<Difficulty> _difficulty = default;
        private readonly EcsCustomInject<Config> _config = default;
        private readonly EcsPoolInject<Enemy> _enemies = default;
        private readonly EcsPoolInject<Health> _healths = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<ShipReference> _references = default;
        private readonly EcsPoolInject<TransformReference> _transformReferences = default;
        private readonly EcsPoolInject<Buffer<Damage>> _buffers = default;
        private readonly EcsPoolInject<AABBCollider> _colliders = default;
        private readonly EcsPoolInject<Weapon> _weapons = default;
        private readonly EcsPoolInject<WeaponInput> _weaponInputs = default;
        private readonly EcsPoolInject<Ship> _ships = default;
        private readonly EcsPoolInject<InsertInCellLists> _cellLists = default;
        private readonly EcsPoolInject<ShipInput> _shipInputs = default;
        private readonly EcsPoolInject<Patrol> _patrols = default;

        private readonly Random _random = new();
        
        public void Run(IEcsSystems systems)
        {
            var enemiesCount = _shipsFilter.Value.GetEntitiesCount();
            var difficulty = _difficulty.Value;
            var shouldSpawn = difficulty.EnemiesCount - enemiesCount;
            var cellLists = _config.Value.CellListsConfig;

            while (shouldSpawn > 0)
            {
                var entity = _world.Value.NewEntity();
                ref var enemy = ref _enemies.Value.Add(entity);
                ref var health = ref _healths.Value.Add(entity);
                ref var transform = ref _transforms.Value.Add(entity);
                ref var transformReference = ref _transformReferences.Value.Add(entity);
                ref var enemyReference = ref _references.Value.Add(entity);
                ref var damageBuffer = ref _buffers.Value.Add(entity);
                ref var collider = ref _colliders.Value.Add(entity);
                ref var weapon = ref _weapons.Value.Add(entity);
                ref var weaponInput = ref _weaponInputs.Value.Add(entity);
                ref var ship = ref _ships.Value.Add(entity);
                ref var patrol = ref _patrols.Value.Add(entity);
                var position = cellLists.RandomPointInside();
                var enemyConfig = difficulty.EnemyConfigs[_random.Next(0, difficulty.EnemyConfigs.Length - 1)];
                var instance = Object.Instantiate(enemyConfig.Prefab);
                _shipInputs.Value.Add(entity);
                _cellLists.Value.Add(entity);

                enemy = enemyConfig.Enemy;
                health.Max = _random.Next(enemyConfig.MinHealth, enemyConfig.MaxHealth);
                health.Min = 0;
                health.Current = health.Max;
                transform.Position = position;
                transform.Rotation = Quaternion.identity;
                transform.Cell = int.MinValue;
                transformReference.Transform = instance.transform;
                enemyReference.View = instance;
                damageBuffer.Initialize();
                collider.Size = enemyConfig.Size;
                collider.Layer = PhysicsLayer.Enemy;
                collider.TargetLayers = PhysicsLayer.Asteroid | PhysicsLayer.Player | PhysicsLayer.Projectile |
                                        PhysicsLayer.Enemy;
                weapon.Damage = new Damage
                {
                    Amount = _random.Next(enemyConfig.MinDamage, enemyConfig.MaxDamage)
                };
                weapon.TransformOffset = 1f;
                weapon.Targets = PhysicsLayer.Player | PhysicsLayer.Enemy;
                weapon.ShootRange = enemyConfig.ShootRange;
                weapon.ProjectileSize = enemyConfig.ProjectileColliderSize;
                weapon.ProjectileSpeed = enemyConfig.ProjectilesSpeed;
                weapon.ProjectilePrefab = enemyConfig.ProjectilePrefab;
                weapon.ShootDelay = enemyConfig.ShootDelay;
                weapon.ReloadTime = enemyConfig.ReloadTime;
                weapon.RoundsMax = enemyConfig.MaxRounds;
                weapon.RoundsLeft = weapon.RoundsMax;
                weaponInput.LookDirection = Vector2.up;
                ship.Acceleration = enemyConfig.Acceleration;
                ship.Damping = enemyConfig.Damping;
                ship.MaxSpeed = enemyConfig.Speed;
                ship.RotationSpeed = enemyConfig.RotationSpeed;
                patrol.Point = _config.Value.CellListsConfig.RandomPointInside();

                shouldSpawn--;
            }
        }
    }
}