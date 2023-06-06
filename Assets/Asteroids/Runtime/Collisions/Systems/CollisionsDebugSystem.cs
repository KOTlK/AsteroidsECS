using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Collision = Asteroids.Runtime.Collisions.Components.Collision;
using Transform = Asteroids.Runtime.CellLists.Components.Transform;

namespace Asteroids.Runtime.Collisions.Systems
{
    public class CollisionsDebugSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Collision>> _filter = "Physics";
        private readonly EcsPoolInject<Collision> _collisions = "Physics";
        private readonly EcsPoolInject<Transform> _transforms = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var collision = ref _collisions.Value.Get(entity);
                ref var firstTransform = ref _transforms.Value.Get(collision.Sender);
                ref var secondTransform = ref _transforms.Value.Get(collision.Receiver);

                Debug.Log(_filter.Value.GetEntitiesCount().ToString());
                Debug.DrawLine(firstTransform.Position, secondTransform.Position, Color.red);
            }
        }
    }
}