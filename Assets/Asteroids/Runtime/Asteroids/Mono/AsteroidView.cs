using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asteroids.Runtime.Asteroids.Mono
{
    public class AsteroidView : MonoBehaviour, IDisposable
    {
        public Vector2 ColliderSize;

        public void Dispose()
        {
            Object.Destroy(gameObject);
        }
    }
}