using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Collisions.Components;
using Asteroids.Runtime.Collisions.Utils;
using Asteroids.Runtime.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Collisions.Systems
{
    public class AABBCollisionDetectionSystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _physicsWorld = Constants.PhysicsWorldName;
        private readonly EcsFilterInject<Inc<Transform, AABBCollider>> _transformsFilter = default;
        private readonly EcsPoolInject<CellNeighbours> _neighbours = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<AABBCollider> _aabbColliders = default;
        private readonly EcsPoolInject<Collision> _collisions = Constants.PhysicsWorldName;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _transformsFilter.Value)
            {
                ref var transform = ref _transforms.Value.Get(entity);
                ref var collider = ref _aabbColliders.Value.Get(entity);
                
                if(transform.Cell == int.MinValue)
                    continue;
                
                ref var neighbours = ref _neighbours.Value.Get(transform.Cell);

                foreach (var neighbourTransformEntity in neighbours.ContainingTransforms)
                {
                    if(entity == neighbourTransformEntity)
                        continue;
                    
                    ref var neighbourTransform = ref _transforms.Value.Get(neighbourTransformEntity);
                    ref var neighbourCollider = ref _aabbColliders.Value.Get(neighbourTransformEntity);
                    
                    if((collider.TargetLayers & neighbourCollider.Layer) != neighbourCollider.Layer)
                        continue;

                    if (CollisionDetection.AABBPair(collider, transform, neighbourCollider, neighbourTransform))
                    {
                        var collisionEntity = _physicsWorld.Value.NewEntity();
                        ref var collision = ref _collisions.Value.Add(collisionEntity);
                        collision = new Collision
                        {
                            Sender = entity,
                            Receiver = neighbourTransformEntity,
                            SenderLayer = collider.Layer,
                            ReceiverLayer = neighbourCollider.Layer
                        };
                    }
                }

                foreach (var neighbourEntity in neighbours.NeighboursEntities)
                {
                    ref var neighbour = ref _neighbours.Value.Get(neighbourEntity);

                    foreach (var neighbourTransformEntity in neighbour.ContainingTransforms)
                    {
                        ref var neighbourTransform = ref _transforms.Value.Get(neighbourTransformEntity);
                        ref var neighbourCollider = ref _aabbColliders.Value.Get(neighbourTransformEntity);
                    
                        if((collider.TargetLayers & neighbourCollider.Layer) != neighbourCollider.Layer)
                            continue;

                        if (CollisionDetection.AABBPair(collider, transform, neighbourCollider, neighbourTransform))
                        {
                            var collisionEntity = _physicsWorld.Value.NewEntity();
                            ref var collision = ref _collisions.Value.Add(collisionEntity);
                            collision = new Collision
                            {
                                Sender = entity,
                                Receiver = neighbourTransformEntity,
                                SenderLayer = collider.Layer,
                                ReceiverLayer = neighbourCollider.Layer
                            };
                        }
                    }
                }
            }
        }
    }
}