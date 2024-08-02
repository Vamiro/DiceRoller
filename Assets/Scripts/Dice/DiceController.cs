using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Dice
{
    public class DiceController : MonoBehaviour
    {
        [SerializeField] private Dice _simulatingDice;
        [SerializeField] private Dice _realDice;
        [SerializeField, Range(1, 15)] private int _speed;
        [SerializeField, Range(1, 15)] private int _simulateSpeed;
        [SerializeField, Range(0f, 100f)] private float _throwForce;
        [SerializeField, Range(0f, 15f)] private float _spinForce;
        
        [SerializeField] private int desiredNum;
        private int _currentNum;
        private bool _isBussy;
        
        void Start()
        {
            _realDice.SetSpeed(_speed);
            _simulatingDice.SetSpeed(1);
            Time.timeScale = _simulateSpeed;
            
            _simulatingDice.DiceLanded += () =>
            {
                _simulatingDice.TransformRecorder.StopRecord();
                _currentNum = _simulatingDice.Result();
            };
            
            _realDice.TransformRecorder.DataHasEnded += () => _isBussy = false;
            
            _simulatingDice.Roll(_throwForce, _spinForce);
            _simulatingDice.TransformRecorder.StartRecord();
        }

        void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_isBussy && !_simulatingDice.TransformRecorder.IsRecording)
            {
                _isBussy = true;
                if(_currentNum != desiredNum)
                {
                    _realDice.TransformRecorder.RotationShift =
                        Quaternion.FromToRotation(GetSideVector(desiredNum), GetSideVector(_currentNum));
                }
                else
                {
                    _realDice.TransformRecorder.RotationShift = Quaternion.identity;
                }

                _realDice.TransformRecorder.CopyValues(_simulatingDice.TransformRecorder);
                _simulatingDice.Roll(_throwForce, _spinForce);
                _simulatingDice.TransformRecorder.StartRecord();
                _realDice.TransformRecorder.StartReplay();
            }
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