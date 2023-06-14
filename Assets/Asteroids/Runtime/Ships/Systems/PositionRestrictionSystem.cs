using Asteroids.Runtime.Application;
using Asteroids.Runtime.CellLists.Components;
using Asteroids.Runtime.Ships.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Ships.Systems
{
    public class PositionRestrictionSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<Config> _config = default;
        private readonly EcsFilterInject<Inc<Ship, Transform, ShipInput>, Exc<Player>> _filter = default;
        private readonly EcsPoolInject<Transform> _transforms = default;
        private readonly EcsPoolInject<ShipInput> _inputs = default;
        
        public void Run(IEcsSystems systems)
        {
            var cellLists = _config.Value.CellListsConfig;
            
            foreach (var entity in _filter.Value)
            {
                ref var transform = ref _transforms.Value.Get(entity);
                ref var input = ref _inputs.Value.Get(entity);

                if (cellLists.Inside(transform.Position) == false)
                {
                    input.MovementDirection = (cellLists.Center - transform.Position).normalized;
                }
            }
        }
    }
}