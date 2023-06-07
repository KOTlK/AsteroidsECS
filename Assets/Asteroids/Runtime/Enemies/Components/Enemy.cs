using Asteroids.Runtime.HP.Components;

namespace Asteroids.Runtime.Enemies.Components
{
    public struct Enemy
    {
        public Damage DamageOnPlayerCollision;
        public int RewardForKill;
        public int LastTakenDamageFrom;
    }
}