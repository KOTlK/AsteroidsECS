using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Time = Asteroids.Runtime.GameTime.Services.Time;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.GameDebug.Systems
{
    public class MoveTransformsSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Time> _time = default;
        private readonly EcsFilterInject<Inc<Transform>> _filter = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var transform = ref _transforms.Value.Get(entity);

                transform.Position += Random.insideUnitCircle.normalized * 5f * _time.Value.DeltaTime;
            }
        }
    }
}