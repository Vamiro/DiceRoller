﻿using UnityEngine;

namespace Dice
{
    [CreateAssetMenu(fileName = "DiceConfig", menuName = "Dice/DiceConfig")]
    public class DiceConfig : ScriptableObject
    {
        [SerializeField, Range(0, 50)] private int _speed;
        private const int _maxSpeed = 50;
        [SerializeField, Range(1, 15)] private int _simulateSpeed;
        [SerializeField, Range(0f, 50f)] private float _throwForce;
        [SerializeField, Range(0f, 50f)] private float _spinForce;
        [SerializeField] private int _numberOfSides;
        [SerializeField] private int _numberOfDices;
        [SerializeField, Range(2, 12)] private int _desiredNum;

        public int Speed => _maxSpeed - _speed;
        public int SimulateSpeed => _simulateSpeed;
        public float ThrowForce => _throwForce;
        public float SpinForce => _spinForce;
        public int NumberOfSides => _numberOfSides;
        public int NumberOfDices => _numberOfDices;
        public int DesiredNum => _desiredNum;
    }
}
