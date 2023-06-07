using Asteroids.Runtime.Application;
using Asteroids.Runtime.Asteroids.Components;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.Transforms.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;
using Time = Asteroids.Runtime.GameTime.Services.Time;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Asteroids.Systems
{
    public class AsteroidsSpawnSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world;
        private readonly EcsCustomInject<Difficulty> _difficulty = default;
        private readonly EcsCustomInject<Config> _config = default;
        private readonly EcsCustomInject<Time> _time = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<TransformReference> _transformReferences = default;
        private readonly EcsPoolInject<Asteroid> _asteroids = default;
        private readonly EcsPoolInject<AsteroidReference> _asteroidReferences = default;
        private readonly EcsPoolInject<InsertInCellLists> _insertCommands = default;
        private readonly EcsPoolInject<AABBCollider> _aabbColliders = default;

        private float _delay;
        private readonly Random _random = new();
        
        public void Run(IEcsSystems systems)
        {
            _delay -= _time.Value.DeltaTime;

            if (_delay <= 0)
            {
                var config = _config.Value;
                var difficulty = _difficulty.Value;
                _delay = _random.Next(difficulty.AsteroidsSpawnMinDelay, difficulty.AsteroidsSpawnMaxDelay);

                for (var i = 0; i < difficulty.AsteroidsAmountPerSpawn; i++)
                {
                    var entity = _world.Value.NewEntity();
                    ref var asteroid = ref _asteroids.Value.Add(entity);
                    ref var transform = ref _transforms.Value.Add(entity);
                    ref var asteroidReference = ref _asteroidReferences.Value.Add(entity);
                    ref var transformReference = ref _transformReferences.Value.Add(entity);
                    ref var collider = ref _aabbColliders.Value.Add(entity);
                    var randomIndex = _random.Next(0, config.AsteroidPrefabs.Length - 1);
                    var instance = Object.Instantiate(config.AsteroidPrefabs[randomIndex]);
                    var asteroidPosition = RandomPosition();
                    _insertCommands.Value.Add(entity);

                    asteroid.Damage = difficulty.AsteroidsDamage;
                    asteroid.Speed = difficulty.AsteroidsSpeed;
                    asteroid.LifeTime = difficulty.AsteroidsLifeTime;
                    asteroid.MovementDirection = (config.CellListsConfig.Center - asteroidPosition).normalized;
                    transform.Position = asteroidPosition;
                    transform.Rotation = Quaternion.identity;
                    asteroidReference.View = instance;
                    transformReference.Transform = instance.transform;
                    collider.Size = instance.ColliderSize;
                    collider.Layer = PhysicsLayer.Asteroid;
                    collider.TargetLayers = PhysicsLayer.Enemy | PhysicsLayer.Player;
                }
            }
        }

        private Vector2 RandomPosition()
        {
            var max = _config.Value.CellListsConfig.Center + _config.Value.CellListsConfig.Size * 0.45f;
            var min = _config.Value.CellListsConfig.Center - _config.Value.CellListsConfig.Size * 0.45f;

            return new Vector2(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y));
        }
    }
}