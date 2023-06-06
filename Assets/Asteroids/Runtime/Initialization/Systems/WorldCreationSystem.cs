using Asteroids.Runtime.Application;
using Asteroids.Runtime.CellLists.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Initialization.Systems
{
    public class WorldCreationSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<Config> _config = default;
        private readonly EcsWorldInject _world = default;
        private readonly EcsPoolInject<CreateCellLists> _cellListsCreationCommands = default;

        public void Init(IEcsSystems systems)
        {
            var entity = _world.Value.NewEntity();
            ref var command = ref _cellListsCreationCommands.Value.Add(entity);
            command = _config.Value.CellListsConfig;
        }
    }
}