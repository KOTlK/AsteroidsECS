using Asteroids.Runtime.GameTime.Systems;
using Asteroids.Runtime.Initialization.Systems;
using Asteroids.Runtime.Input.Systems;
using Asteroids.Runtime.Ships.Systems;
using Asteroids.Runtime.Transforms.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Mitfart.LeoECSLite.UnityIntegration.Plugins.Mitfart.LeoECSLite.UnityIntegration.Runtime;//wtf
using UnityEngine;
using Time = Asteroids.Runtime.GameTime.Services.Time;
using Transform = Asteroids.Runtime.Transforms.Components.Transform;

namespace Asteroids.Runtime.Application
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private Config _config;
        
        private EcsSystems _systems;

        private void Start()
        {
            var world = new EcsWorld();
            _systems = new EcsSystems(world);

#if UNITY_EDITOR

            var debugEntity = world.NewEntity(); // entity for correct debug visualization
            var somePool = world.GetPool<Transform>();
            somePool.Add(debugEntity);
            
#endif

            AddInitSystems();
            _systems
                .Add(new TimeSystem())
                .Add(new PlayerShipInputSystem())
                .Add(new ShipMovementSystem())
                .Add(new VelocitySystem())
                .Add(new SyncTransformSystem())

#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                
                .Inject(new Time(), _config)
                .Init();
        }

        private void AddInitSystems()
        {
            _systems
                .Add(new PlayerInitializationSystem())
                
                ;
        }

        private void Update()
        {
            _systems.Run();
        }

        private void OnDestroy()
        {
            _systems.Destroy();
        }
    }
}