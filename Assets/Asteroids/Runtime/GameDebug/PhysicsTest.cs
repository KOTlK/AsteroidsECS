using Asteroids.Runtime.Application;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.CellLists.Systems;
using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.Collisions.Systems;
using Asteroids.Runtime.GameDebug.Systems;
using Asteroids.Runtime.GameTime.Systems;
using Asteroids.Runtime.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using TMPro;
using UnityEngine;
using Collision = Asteroids.Runtime.Collisions.Components.Collision;
using Random = UnityEngine.Random;
using Time = Asteroids.Runtime.GameTime.Services.Time;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.GameDebug
{
    public class PhysicsTest : MonoBehaviour
    {
        [SerializeField] private CellListsConfig _cellListsConfig;
        [SerializeField] private int _collidersPerSpawn = 500;
        [SerializeField] private TMP_Text _amountText;
        
        private EcsSystems _systems;
        private int _collidersAmount;

        private void Start()
        {
            UnityEngine.Application.targetFrameRate = 0;
            var world = new EcsWorld();
            var physicsWorld = new EcsWorld();
            _systems = new EcsSystems(world);
            _systems.AddWorld(physicsWorld, Constants.PhysicsWorldName);

            var debugEntity = world.NewEntity(); // entity for correct debug visualization
            var transformsPool = world.GetPool<Transform>();
            transformsPool.Add(debugEntity);

            _systems
                .Add(new CellListsInitSystem())
                .Add(new TimeSystem())
                .Add(new InsertTransformSystem())
                .Add(new MoveTransformsSystem())
                //.Add(new RemoveFromCellListsSystem())
                .Add(new CellListsRebuildSystem())
                .Add(new CellDrawSystem())
                //.Add(new CellNeighboursDrawSystem())
                .Add(new DisplayNeighboursSystem())
                .DelHere<Collision>(Constants.PhysicsWorldName)
                .Add(new AABBCollisionDetectionSystem())
                .Add(new CollisionsDebugSystem())
                .Inject(new Time(), new Config(){CellListsConfig = _cellListsConfig})
                .Init();
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                SpawnColliders(_collidersPerSpawn);
            }
            _systems.Run();
        }

        private void SpawnColliders(int amount)
        {
            var world = _systems.GetWorld();
            var transforms = world.GetPool<Transform>();
            var colliders = world.GetPool<AABBCollider>();
            var insertPool = world.GetPool<InsertInCellLists>();
            
            for (var i = 0; i < amount; i++)
            {
                var entity = world.NewEntity();
                ref var transformComponent = ref transforms.Add(entity);
                ref var colliderComponent = ref colliders.Add(entity);
                insertPool.Add(entity);

                transformComponent.Position = RandomPosition();
                colliderComponent.Size = Vector2.one;
                colliderComponent.Layer = PhysicsLayer.Enemy;
                colliderComponent.TargetLayers = PhysicsLayer.Enemy;
            }

            _collidersAmount += amount;
            _amountText.text = _collidersAmount.ToString();
        }

        private Vector2 RandomPosition()
        {
            var max = _cellListsConfig.Center + _cellListsConfig.Size * 0.45f;
            var min = _cellListsConfig.Center - _cellListsConfig.Size * 0.45f;

            return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }
    }
}