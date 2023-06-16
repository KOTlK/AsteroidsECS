using System;
using System.Collections;
using UnityEngine;

namespace Asteroids.Runtime.Ships.Mono
{
    public class ShipView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _explosionTime = 1f;

        private static readonly int ExplosionHash = Animator.StringToHash("ShipExplosion");
        
        public void Dispose()
        {
            _animator.Play(ExplosionHash);
            StartCoroutine(Destroying(_explosionTime));
        }
        
        private IEnumerator Destroying(float time)
        {
            yield return new WaitForSeconds(time);

            Destroy(gameObject);
        }
    }
}