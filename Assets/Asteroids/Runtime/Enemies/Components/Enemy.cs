using System;
using Asteroids.Runtime.HP.Components;

namespace Asteroids.Runtime.Enemies.Components
{
    [Serializable]
    public struct Enemy
    {
        public Damage DamageOnPlayerCollision;
        public int RewardForKill;
        public int LastTakenDamageFrom;
        public float FollowRange;
        public float SafeDistance;
    }
}