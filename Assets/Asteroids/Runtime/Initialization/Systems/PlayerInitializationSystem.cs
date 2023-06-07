using System.Collections.Generic;
using Asteroids.Runtime.Application;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Input.Components;
using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.Transforms.Components;
using Asteroids.Runtime.Utils.Components;
using Asteroids.Runtime.Weapons.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Initialization.Systems
{
    public class PlayerInitializationSystem : IEcsInitSystem
    {
        private readonly EcsWorldInject _world;
        private readonly EcsCustomInject<Config> _config = default;
        private readonly EcsPoolInject<Player> _players = default;
        private readonly EcsPoolInject<Ship> _ships = default;
        private readonly EcsPoolInject<ShipInput> _shipInputs = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<TransformReference> _transformReferences = default;
        private readonly EcsPoolInject<ShipView> _views = default;
        private readonly EcsPoolInject<AABBCollider> _colliders = default;
        private readonly EcsPoolInject<InsertInCellLists> _insertInCellLists = default;
        private readonly EcsPoolInject<Health> _healths = default;
        private readonly EcsPoolInject<Buffer<Damage>> _damageBuffers = default;
        private readonly EcsPoolInject<Weapon> _weapons = default;
        private readonly EcsPoolInject<WeaponInput> _weaponInputs = default;
        
        public void Init(IEcsSystems systems)
        {
            var config = _config.Value;
            var entity = _world.Value.NewEntity();
            var instance = Object.Instantiate(config.PlayerShipPrefab, config.CellListsConfig.Center, Quaternion.identity);
            ref var ship = ref _ships.Value.Add(entity);
            ref var transform = ref _transforms.Value.Add(entity);
            ref var transformReference = ref _transformReferences.Value.Add(entity);
            ref var view = ref _views.Value.Add(entity);
            ref var collider = ref _colliders.Value.Add(entity);
            ref var health = ref _healths.Value.Add(entity);
            ref var damageBuffer = ref _damageBuffers.Value.Add(entity);
            ref var weapon = ref _weapons.Value.Add(entity);
            ref var weaponInput = ref _weaponInputs.Value.Add(entity);
            _players.Value.Add(entity);
            _shipInputs.Value.Add(entity);
            _insertInCellLists.Value.Add(entity);

            ship = config.PlayerShipConfig;
            transform.Position = config.CellListsConfig.Center;
            transform.Rotation = Quaternion.identity;
            transformReference.Transform = instance.transform;
            view.GameObject = instance;
            collider.Size = new Vector2(1, 1);
            collider.Layer = PhysicsLayer.Player;
            collider.TargetLayers = PhysicsLayer.Enemy | PhysicsLayer.Asteroid;
            health.Max = config.PlayerHealth;
            health.Current = health.Max;
            health.Min = 0;
            damageBuffer.Initialize();
            weapon = config.PlayerWeaponConfig;
            weaponInput.LookDirection = Vector2.up;
        }
    }
}