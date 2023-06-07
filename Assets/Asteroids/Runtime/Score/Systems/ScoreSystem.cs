using Asteroids.Runtime.Asteroids.Components;
using Asteroids.Runtime.Ships.Components;
using Asteroids.Runtime.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Asteroids.Runtime.Score.Systems
{
    public class ScoreSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<GameScore> _score = default;
        private readonly EcsWorldInject _eventsWorld = Constants.EventsWorldName;
        private readonly EcsFilterInject<Inc<AsteroidDestroyed>> _asteroidsDestroyedFilter = Constants.EventsWorldName;
        private readonly EcsPoolInject<AsteroidDestroyed> _asteroidsDestroyed = Constants.EventsWorldName;
        private readonly EcsPoolInject<Player> _players = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _asteroidsDestroyedFilter.Value)
            {
                ref var command = ref _asteroidsDestroyed.Value.Get(entity);

                if (_players.Value.Has(command.Destroyer))
                {
                    _score.Value.Add(command.Reward);
                }

                _eventsWorld.Value.DelEntity(entity);
            }
        }
    }
}