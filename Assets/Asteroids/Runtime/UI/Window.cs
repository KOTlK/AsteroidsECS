using UnityEngine;

namespace Asteroids.Runtime.UI
{
    public class Window : MonoBehaviour
    {
        public bool IsActive
        {
            get => gameObject.activeSelf;
            set
            {
                if (IsActive != value)
                {
                    gameObject.SetActive(value);
                }
            }
        }
    }
}