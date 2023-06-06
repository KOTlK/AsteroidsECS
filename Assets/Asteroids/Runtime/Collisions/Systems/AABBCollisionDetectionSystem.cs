using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.Collisions.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Collisions.Systems
{
    public class AABBCollisionDetectionSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _physicsWorld = "Physics";
        private readonly EcsFilterInject<Inc<Cell, CellNeighbours>> _filter = default;
        private readonly EcsPoolInject<CellNeighbours> _neighbours = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<AABBCollider> _aabbColliders = default;
        private readonly EcsPoolInject<Collision> _collisions = "Physics";

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var neighbours = ref _neighbours.Value.Get(entity);

                foreach (var transformEntity in neighbours.ContainingTransforms)
                {
                    ref var transform = ref _transforms.Value.Get(transformEntity);
                    ref var collider = ref _aabbColliders.Value.Get(transformEntity);

                    foreach (var secondTransformEntity in neighbours.ContainingTransforms)
                    {
                        if(transformEntity == secondTransformEntity)
                            continue;
                        
                        ref var secondTransform = ref _transforms.Value.Get(secondTransformEntity);
                        ref var secondCollider = ref _aabbColliders.Value.Get(secondTransformEntity);
                        
                        if((collider.TargetLayers & secondCollider.Layer) != secondCollider.Layer)
                            continue;
                    
                        if (CollisionDetection.AABBPair(collider, transform, secondCollider, secondTransform))
                        {
                            var collisionEntity = _physicsWorld.Value.NewEntity();
                            ref var collision = ref _collisions.Value.Add(collisionEntity);
                            collision = new Collision
                            {
                                Sender = transformEntity,
                                Receiver = secondTransformEntity,
                                SenderLayer = collider.Layer,
                                ReceiverLayer = secondCollider.Layer
                            };
                        }
                    }

                    foreach (var neighbourEntity in neighbours.NeighboursEntities)
                    {
                        ref var neighbour = ref _neighbours.Value.Get(neighbourEntity);

                        foreach (var secondTransformEntity in neighbour.ContainingTransforms)
                        {
                            if(transformEntity == secondTransformEntity)
                                continue;
                        
                            ref var secondTransform = ref _transforms.Value.Get(secondTransformEntity);
                            ref var secondCollider = ref _aabbColliders.Value.Get(secondTransformEntity);
                        
                            if((collider.TargetLayers & secondCollider.Layer) != secondCollider.Layer)
                                continue;
                    
                            if (CollisionDetection.AABBPair(collider, transform, secondCollider, secondTransform))
                            {
                                var collisionEntity = _physicsWorld.Value.NewEntity();
                                ref var collision = ref _collisions.Value.Add(collisionEntity);
                                collision = new Collision
                                {
                                    Sender = transformEntity,
                                    Receiver = secondTransformEntity,
                                    SenderLayer = collider.Layer,
                                    ReceiverLayer = secondCollider.Layer
                                };
                            }
                        }
                    }
                }
        
                /*
                Filling.Begin();
                ref var neighbours = ref _neighbours.Value.Get(entity);
                var closest = new NativeList<(Transform, AABBCollider, int)>(Allocator.TempJob);
                var outputCollisions = new NativeQueue<Collision>(Allocator.TempJob);

                foreach (var transformEntity in neighbours.ContainingTransforms)
                {
                    ref var transform = ref _transforms.Value.Get(transformEntity);
                    ref var aabb = ref _aabbColliders.Value.Get(transformEntity);
                    closest.Add((transform, aabb, transformEntity));
                }

                foreach (var neighbourEntity in neighbours.NeighboursEntities)
                {
                    ref var neighbour = ref _neighbours.Value.Get(neighbourEntity);
                    foreach (var transformEntity in neighbour.ContainingTransforms)
                    {
                        ref var transform = ref _transforms.Value.Get(transformEntity);
                        ref var aabb = ref _aabbColliders.Value.Get(transformEntity);
                        closest.Add((transform, aabb, transformEntity));
                    }
                }
                Filling.End();

                Job.Begin();
                var job = new CollisionsJob()
                {
                    ClosestTransforms = closest,
                    Output = outputCollisions.AsParallelWriter()
                };
                
                job.Schedule(closest.Length, 1).Complete();
                
                Job.End();
                
                Finalization.Begin();
                while (outputCollisions.Count > 0)
                {
                    var collision = outputCollisions.Dequeue();
                    if(collision is { Sender: 0, Receiver: 0 })
                        continue;
                    
                    var collisionEntity = _physicsWorld.Value.NewEntity();
                    ref var collisionEvent = ref _collisions.Value.Add(collisionEntity);
                    collisionEvent = collision;
                }

                closest.Dispose();
                outputCollisions.Dispose();
                Finalization.End();
                */
            }
        }
/*
        [BurstCompile]
        private struct CollisionsJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<(Transform, AABBCollider, int)> ClosestTransforms;
            [WriteOnly] public NativeQueue<Collision>.ParallelWriter Output;

            [BurstCompile]
            public void Execute(int index)
            {
                var (transform, collider, entity) = ClosestTransforms[index];

                foreach (var (secondTransform, secondCollider, secondEntity) in ClosestTransforms)
                {
                    if (entity == secondEntity)
                        continue;

                    if((collider.TargetLayers & secondCollider.Layer) != secondCollider.Layer)
                        continue;
                    
                    if (CollisionDetection.AABBPair(collider, transform, secondCollider, secondTransform))
                    {
                        Output.Enqueue(new Collision
                        {
                            Sender = entity,
                            Receiver = secondEntity,
                            SenderLayer = collider.Layer,
                            ReceiverLayer = secondCollider.Layer
                        });
                    }
                }
            }
        }
        */
    }
}