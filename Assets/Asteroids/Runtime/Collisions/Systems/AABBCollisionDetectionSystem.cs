using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.Collisions.Utils;
using Asteroids.Runtime.Transforms.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
//using Unity.Profiling;

namespace Asteroids.Runtime.Collisions.Systems
{
    //Inefficient 3000 colliders - 350+kb allocations ))))00)), 2.5ms per job
    //With layers job takes 1.5ms, filling array takes 1.1ms
    //With 3 nativearrays instead 1, filling takes 0.6ms, job takes 5ms
    public class AABBCollisionDetectionSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _physicsWorld = "Physics";
        private readonly EcsFilterInject<Inc<AABBCollider, Transform>> _filter = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<AABBCollider> _aabbColliders = default;
        private readonly EcsPoolInject<Collision> _collisions = "Physics";

        //private static readonly ProfilerMarker Initialization = new (nameof(Initialization));
        //private static readonly ProfilerMarker Filling = new (nameof(Filling));
        //private static readonly ProfilerMarker Job = new (nameof(Job));

        public void Run(IEcsSystems systems)
        {
            //Initialization.Begin();
            var length = _filter.Value.GetEntitiesCount();
            var array = new NativeArray<(Transform, AABBCollider, int)>(length, Allocator.TempJob);
            var index = 0;
            var collisions = new NativeQueue<Collision>(Allocator.TempJob);
            //Initialization.End();

            //Filling.Begin();
            foreach (var entity in _filter.Value)
            {
                ref var transform = ref _transforms.Value.Get(entity);
                ref var collider = ref _aabbColliders.Value.Get(entity);

                array[index] = (transform, collider, entity);
                
                index++;
            }
            //Filling.End();

            //Job.Begin();
            var job = new CollisionsJob()
            {
                Targets = array,
                Collisions = collisions.AsParallelWriter()
            };

            job.Schedule(length, 32).Complete();

            while (collisions.Count > 0)
            {
                var collision = collisions.Dequeue();
                if(collision is { Sender: 0, Receiver: 0 })
                    continue;
                    
                var collisionEntity = _physicsWorld.Value.NewEntity();
                ref var collisionEvent = ref _collisions.Value.Add(collisionEntity);
                collisionEvent = collision;
            }
            
            array.Dispose();
            collisions.Dispose();
            //Job.End();
        }

        [BurstCompile]
        private struct CollisionsJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<(Transform, AABBCollider, int)> Targets;
            public NativeQueue<Collision>.ParallelWriter Collisions;
            
            [BurstCompile]
            public void Execute(int index)
            {
                var (transform, collider, entity) = Targets[index];

                foreach (var (secondTransform, secondCollider, secondEntity) in Targets)
                {
                    if (entity == secondEntity)
                        continue;

                    if((collider.TargetLayers & secondCollider.Layer) != secondCollider.Layer)
                        continue;
                    
                    if (CollisionDetection.AABBPair(collider, transform, secondCollider, secondTransform))
                    {
                        Collisions.Enqueue(new Collision()
                        {
                            Sender = entity,
                            Receiver = secondEntity
                        });
                    }
                }
            }
        }
    }
}