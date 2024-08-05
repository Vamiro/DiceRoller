using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = System.Random;

namespace Dice
{
    public class DiceController : MonoBehaviour
    {
        public bool CanRoll => !_isBusy && !_simulatingDice.TransformRecorder.IsRecording;
        [SerializeField] private Dice _simulatingDice;
        [SerializeField] private Dice _realDice;

        private int _speed;
        private float _throwForce;
        private float _spinForce;
        private int _currentNum;
        private bool _isBusy;
        
        [Inject]
        public void Construct(DiceConfig config)
        {
            Time.timeScale = config.SimulateSpeed;
            _speed = config.Speed;
            _throwForce = config.ThrowForce;
            _spinForce = config.SpinForce;
        }
        
        void Start()
        {
            _realDice.Initialize(transform, 51 - _speed);
            _simulatingDice.Initialize(transform, 1);
            
            _simulatingDice.DiceLanded += () =>
            {
                _currentNum = _simulatingDice.Result();
                if (_currentNum == -1) SimulateRoll();
                else _simulatingDice.TransformRecorder.StopRecord();
            };
            
            _realDice.TransformRecorder.DataHasEnded += () => _isBusy = false;
            
            SimulateRoll();
        }
        
        public void PrecalculatedRoll(int desiredNum)
        {
             if (CanRoll)
             {
                 _isBusy = true;
                 if(_currentNum != desiredNum)
                 {
                     _realDice.TransformRecorder.RotationShift =
                         Quaternion.FromToRotation(GetSideVector(desiredNum), GetSideVector(_currentNum));
                 }
                 else if(_currentNum == desiredNum)
                 {
                     _realDice.TransformRecorder.RotationShift = Quaternion.identity;
                 }

                 _realDice.TransformRecorder.CopyValues(_simulatingDice.TransformRecorder);
                 SimulateRoll();
                 _realDice.TransformRecorder.StartReplay();
             }
        }

        private void SimulateRoll()
        {
            _simulatingDice.Roll(_throwForce, _spinForce);
            _simulatingDice.TransformRecorder.StartRecord();
        }

        private Vector3 GetSideVector(int num)
        {
            switch (num)
            {
                case 1: return Vector3.forward;
                case 2: return Vector3.up;
                case 3: return Vector3.left;
                case 4: return Vector3.right;
                case 5: return Vector3.down;
                case 6: return Vector3.back;
                default: return Vector3.zero;
            }
        }
    }
}