using Leopotam.EcsLite;

namespace Asteroids.Runtime.Application.GameLoop
{
    public abstract class State
    {
        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Execute()
        {
        }
    }
}