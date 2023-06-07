using System;
using UnityEngine;

namespace Asteroids.Runtime.Weapons.Mono
{
    public class ProjectileView : MonoBehaviour, IDisposable
    {
        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}