using Asteroids.Runtime.Collisions.Systems;
using Asteroids.Runtime.GameTime.Systems;
using Asteroids.Runtime.Initialization.Systems;
using Asteroids.Runtime.Input.Systems;
using Asteroids.Runtime.Ships.Systems;
using Asteroids.Runtime.Transforms.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Mitfart.LeoECSLite.UnityIntegration.Plugins.Mitfart.LeoECSLite.UnityIntegration.Runtime;
using Mitfart.LeoECSLite.UnityIntegration.Plugins.Mitfart.LeoECSLite.UnityIntegration.Runtime.Name; //wtf
using UnityEngine;
using Collision = Asteroids.Runtime.Collisions.Components.Collision;
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
            UnityEngine.Application.targetFrameRate = 0;
            var world = new EcsWorld();
            var physicsWorld = new EcsWorld();
            _systems = new EcsSystems(world);
            _systems.AddWorld(physicsWorld, "Physics");

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
                .DelHere<Collision>("Physics")
                .Add(new AABBCollisionDetectionSystem())
                .Add(new SyncTransformSystem())

#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
                .Add(new EcsWorldDebugSystem("Physics", new NameSettings(true)))
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