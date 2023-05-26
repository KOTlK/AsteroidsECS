using Asteroids.Runtime.Transforms.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Unity.Collections;
using UnityEngine.Jobs;
using Transform = Asteroids.Runtime.Transforms.Components.Transform;

namespace Asteroids.Runtime.Transforms.Systems
{
    public class SyncTransformSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Transform, TransformReference>> _filter = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<TransformReference> _references = default;
        private NativeArray<Transform> _transformsArray = new (10000, Allocator.Persistent);

        public void Run(IEcsSystems systems)
        {
            var filter = _filter.Value;
            var accessArray = new TransformAccessArray(filter.GetEntitiesCount());
            var index = 0;

            foreach (var entity in filter)
            {
                ref var transform = ref _transforms.Value.Get(entity);
                ref var reference = ref _references.Value.Get(entity);

                accessArray.Add(reference.Transform);
                _transformsArray[index] = transform;
                index++;
            }

            var job = new SyncJob()
            {
                Transforms = _transformsArray
            };
            
            job.Schedule(accessArray).Complete();

            accessArray.Dispose();
        }
        
        private struct SyncJob : IJobParallelForTransform
        {
            [ReadOnly] public NativeArray<Transform> Transforms;
            
            public void Execute(int index, TransformAccess transform)
            {
                var target = Transforms[index];
                transform.localPosition = target.Position;
                transform.localRotation = target.Rotation;
            }
        }
    }
}