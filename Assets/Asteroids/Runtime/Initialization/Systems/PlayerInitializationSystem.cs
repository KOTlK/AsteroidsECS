using Asteroids.Runtime.Application;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.HP.Components;
using Asteroids.Runtime.Settings.PlayerConfigs;
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
    public class PlayerInitializationSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private readonly EcsWorldInject _world;
        private readonly EcsCustomInject<PlayerConfig> _playerConfig = default;
        private readonly EcsCustomInject<Config> _config = default;
        private readonly EcsPoolInject<Player> _players = default;
        private readonly EcsPoolInject<Ship> _ships = default;
        private readonly EcsPoolInject<ShipInput> _shipInputs = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<TransformReference> _transformReferences = default;
        private readonly EcsPoolInject<ShipReference> _views = default;
        private readonly EcsPoolInject<AABBCollider> _colliders = default;
        private readonly EcsPoolInject<InsertInCellLists> _insertInCellLists = default;
        private readonly EcsPoolInject<Health> _healths = default;
        private readonly EcsPoolInject<Buffer<Damage>> _damageBuffers = default;
        private readonly EcsPoolInject<Weapon> _weapons = default;
        private readonly EcsPoolInject<WeaponInput> _weaponInputs = default;
        
        public void Init(IEcsSystems systems)
        {
            var config = _config.Value;
            var playerConfig = _playerConfig.Value;
            var entity = _world.Value.NewEntity();
            var instance = Object.Instantiate(playerConfig.PlayerShipPrefab, config.CellListsConfig.Center, Quaternion.identity);
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

            ship = playerConfig.PlayerShipConfig;
            transform.Position = config.CellListsConfig.Center;
            transform.Rotation = Quaternion.identity;
            transform.Cell = int.MinValue;
            transformReference.Transform = instance.transform;
            view.View = instance;
            collider.Size = new Vector2(1, 1);
            collider.Layer = PhysicsLayer.Player;
            collider.TargetLayers = PhysicsLayer.Enemy | PhysicsLayer.Asteroid;
            health.Max = playerConfig.PlayerHealth;
            health.Current = health.Max;
            health.Min = 0;
            damageBuffer.Initialize();
            weapon = playerConfig.PlayerWeaponConfig;
            weaponInput.LookDirection = Vector2.up;
        }


        private readonly EcsFilterInject<Inc<ShipReference>> _filter = default;
        
        public void Destroy(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var reference = ref _views.Value.Get(entity);
                reference.Dispose();
            }
        }
    }
}