using Asteroids.Runtime.Asteroids.Systems;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.CellLists.Systems;
using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.Collisions.Systems;
using Asteroids.Runtime.GameTime.Systems;
using Asteroids.Runtime.HP.Systems;
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
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Application
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private Config _config;
        [SerializeField] private Difficulty _difficulty;
        
        private EcsSystems _systems;

        private void Start()
        {
            UnityEngine.Application.targetFrameRate = 0;
            var world = new EcsWorld();
            var physicsWorld = new EcsWorld();
            _systems = new EcsSystems(world);
            _systems.AddWorld(physicsWorld, "Physics");


            var debugEntity = world.NewEntity(); // entity for correct debug visualization
            var transformsPool = world.GetPool<Transform>();
            transformsPool.Add(debugEntity);
            

            Vector2 Random()
            {
                return UnityEngine.Random.insideUnitCircle * 50;
            }
            
            var aabbsPool = world.GetPool<AABBCollider>();
            var insertPool = world.GetPool<InsertInCellLists>();


            AddInitSystems();
            _systems
                .Add(new TimeSystem())
                .Add(new AsteroidsSpawnSystem())
                .Add(new PlayerShipInputSystem())
                .Add(new ShipMovementSystem())
                .Add(new VelocitySystem())
                .Add(new AsteroidsMovementSystem())
                .Add(new AsteroidsLifeSystem())
                .Add(new CellListsInitSystem())
                .Add(new InsertTransformSystem())
                .Add(new CellListsRebuildSystem())
                .Add(new CellDrawSystem())
                .DelHere<Collision>("Physics")
                .Add(new AABBCollisionDetectionSystem())
                //.Add(new CollisionsDebugSystem())
                .Add(new CollisionsHandleSystem())
                .Add(new DamageSystem())
                .Add(new AsteroidsDestroySystem())
                .Add(new SyncTransformSystem())

#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
                .Add(new EcsWorldDebugSystem("Physics", new NameSettings(true)))
#endif
                
                .Inject(new Time(), _config, _difficulty)
                .Init();


            /*for (var i = 0; i < 1000; i++)
            {
                var entity = world.NewEntity();
                ref var transform1 = ref transformsPool.Add(entity);
                ref var aabb = ref aabbsPool.Add(entity);
                var position = Random();
                insertPool.Add(entity);
                transform1.Position = position;
                transform1.Rotation = Quaternion.identity;
                aabb.Layer = PhysicsLayer.Enemy;
                aabb.TargetLayers = PhysicsLayer.Player | PhysicsLayer.Asteroid;
                aabb.Size = new Vector2(1, 1);
                Debug.DrawRay(transform1.Position, Vector3.up, Color.blue, 200f);
            }*/
        }

        private void AddInitSystems()
        {
            _systems
                .Add(new WorldCreationSystem())
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