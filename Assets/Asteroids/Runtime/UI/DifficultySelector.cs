using System;
using Asteroids.Runtime.Application;
using TMPro;
using UnityEngine;

namespace Asteroids.Runtime.UI
{
    public class DifficultySelector : Window
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private Difficulty _easy, _medium, _hard;

        public Difficulty Selected
        {
            get
            {
                return _dropdown.value switch
                {
                    0 => _easy,
                    1 => _medium,
                    2 => _hard,
                    _ => throw new ArgumentException()
                };
            }
        }
    }
}