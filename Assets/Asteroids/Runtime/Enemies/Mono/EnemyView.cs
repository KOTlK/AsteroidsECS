using System;
using UnityEngine;

namespace Asteroids.Runtime.Enemies.Mono
{
    public class EnemyView : MonoBehaviour, IDisposable
    {
        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}