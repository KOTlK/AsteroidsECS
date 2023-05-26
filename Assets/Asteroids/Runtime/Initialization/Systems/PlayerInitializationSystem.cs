using Asteroids.Runtime.Application;
using Asteroids.Runtime.Input.Components;
using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.Transforms.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Transform = Asteroids.Runtime.Transforms.Components.Transform;

namespace Asteroids.Runtime.Initialization.Systems
{
    public class PlayerInitializationSystem : IEcsInitSystem
    {
        private readonly EcsWorldInject _world;
        private readonly EcsPoolInject<Player> _players = default;
        private readonly EcsPoolInject<Ship> _ships = default;
        private readonly EcsPoolInject<ShipInput> _shipInputs = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<TransformReference> _transformReferences = default;
        private readonly EcsPoolInject<ShipView> _views = default;
        private readonly EcsCustomInject<Config> _config = default;
        
        public void Init(IEcsSystems systems)
        {
            var config = _config.Value;
            var entity = _world.Value.NewEntity();
            var instance = Object.Instantiate(config.PlayerShipPrefab, Vector2.zero, Quaternion.identity);
            ref var ship = ref _ships.Value.Add(entity);
            ref var transform = ref _transforms.Value.Add(entity);
            ref var transformReference = ref _transformReferences.Value.Add(entity);
            ref var view = ref _views.Value.Add(entity);
            _players.Value.Add(entity);
            _shipInputs.Value.Add(entity);

            ship = config.PlayerShipConfig;
            transform.Position = Vector2.zero;
            transform.Rotation = Quaternion.identity;
            transformReference.Transform = instance.transform;
            view.GameObject = instance;
        }
    }
}