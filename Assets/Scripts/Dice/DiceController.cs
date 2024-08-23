using UnityEngine;
using Zenject;

namespace Dice
{
    public class DiceController : MonoBehaviour
    {
        public bool CanRoll => !_isBusy && !_simulatingDiceRecorder.IsRecording;
        
        [SerializeField] private DiceComponent simulatingDiceComponent;
        [SerializeField] private TransformRecorder _simulatingDiceRecorder;
        
        [SerializeField] private DiceComponent realDiceComponent;
        [SerializeField] private TransformReplayer _realDiceReplayer;

        private int _speed;
        private int _maxSpeed;
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
            realDiceComponent.Initialize(transform);
            simulatingDiceComponent.Initialize(transform);
            
            //_simulatingDiceRecorder.SetNumberOfSkippedFrames(1);
            _realDiceReplayer.SetNumberOfSkippedFrames(_speed);
            
            simulatingDiceComponent.OnDiceLanded -= OnDiceComponentLanded;
            simulatingDiceComponent.OnDiceLanded += OnDiceComponentLanded;
            
            _realDiceReplayer.DataHasEnded -= OnReplayEnded;
            _realDiceReplayer.DataHasEnded += OnReplayEnded;
            
            SimulateRoll();
        }

        private void OnDestroy()
        {
            _realDiceReplayer.DataHasEnded -= OnReplayEnded;
            simulatingDiceComponent.OnDiceLanded -= OnDiceComponentLanded;
        }

        public void PrecalculatedRoll(int desiredNum)
        {
             if (CanRoll)
             {
                 _isBusy = true;
                 if(_currentNum != desiredNum)
                 {
                     _realDiceReplayer.RotationShift =
                         Quaternion.FromToRotation(DiceComponent._diceSides[desiredNum - 1], DiceComponent._diceSides[_currentNum - 1]);
                 }
                 else if(_currentNum == desiredNum)
                 {
                     _realDiceReplayer.RotationShift = Quaternion.identity;
                 }

                 _realDiceReplayer.StartReplay(_simulatingDiceRecorder.Buffer);
                 SimulateRoll();
             }
        }

        private void OnDiceComponentLanded()
        {
            _currentNum = simulatingDiceComponent.Result();
            if (_currentNum == -1) SimulateRoll();
            else _simulatingDiceRecorder.StopRecord();
        }
        
        private void OnReplayEnded()
        {
            _isBusy = false;
        }

        private void SimulateRoll()
        {
            simulatingDiceComponent.Roll(_throwForce, _spinForce);
            _simulatingDiceRecorder.StartRecord();
        }
    }
}
