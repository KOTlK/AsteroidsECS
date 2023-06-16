using Leopotam.EcsLite;

namespace Asteroids.Runtime.Application.GameLoop
{
    public class RunningSystems
    {
        private EcsSystems _systems;

        public void Switch(EcsSystems newSystems)
        {
            _systems = newSystems;
        }

        public EcsSystems Systems => _systems;
        
        public void Execute()
        {
            _systems.Run();
        }

        public void Destroy()
        {
            _systems.Destroy();
        }
    }
}