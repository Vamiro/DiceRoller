using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Dice
{
    public class DiceController : MonoBehaviour
    {
        public bool CanRoll => !_isBusy && !simulatingDiceComponent.TransformRecorder.IsRecording;
        [SerializeField] private DiceComponent simulatingDiceComponent;
        [SerializeField] private DiceComponent realDiceComponent;

        private int _speed;
        private float _throwForce;
        private float _spinForce;
        private int _currentNum;
        private TransformRecorder _realDiceTransformRecorder;
        private TransformRecorder _simulatingDiceTransformRecorder;
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
            realDiceComponent.Initialize(transform, 51 - _speed);
            simulatingDiceComponent.Initialize(transform, 1);

            _realDiceTransformRecorder = realDiceComponent.TransformRecorder;
            _simulatingDiceTransformRecorder = simulatingDiceComponent.TransformRecorder;
            
            simulatingDiceComponent.OnDiceLanded -= OnDiceComponentLanded;
            simulatingDiceComponent.OnDiceLanded += OnDiceComponentLanded;
            
            _realDiceTransformRecorder.DataHasEnded -= OnReplayEnded;
            _realDiceTransformRecorder.DataHasEnded += OnReplayEnded;
            
            SimulateRoll();
        }

        private void OnDestroy()
        {
            _realDiceTransformRecorder.DataHasEnded -= OnReplayEnded;
            simulatingDiceComponent.OnDiceLanded -= OnDiceComponentLanded;
        }

        public void PrecalculatedRoll(int desiredNum)
        {
             if (CanRoll)
             {
                 _isBusy = true;
                 if(_currentNum != desiredNum)
                 {
                     _realDiceTransformRecorder.RotationShift =
                         Quaternion.FromToRotation(GetSideVector(desiredNum), GetSideVector(_currentNum));
                 }
                 else if(_currentNum == desiredNum)
                 {
                     _realDiceTransformRecorder.RotationShift = Quaternion.identity;
                 }

                 _realDiceTransformRecorder.CopyValues(_simulatingDiceTransformRecorder);
                 SimulateRoll();
                 _realDiceTransformRecorder.StartReplay();
             }
        }

        private void OnDiceComponentLanded()
        {
            _currentNum = simulatingDiceComponent.Result();
            if (_currentNum == -1) SimulateRoll();
            else _simulatingDiceTransformRecorder.StopRecord();
        }
        
        private void OnReplayEnded()
        {
            _isBusy = false;
        }

        private void SimulateRoll()
        {
            simulatingDiceComponent.Roll(_throwForce, _spinForce);
            _simulatingDiceTransformRecorder.StartRecord();
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