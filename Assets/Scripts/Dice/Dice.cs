using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dice
{
    [RequireComponent(typeof(TransformRecorder))]
    public class Dice : MonoBehaviour
    {
        private Rigidbody _rb;
        private bool _hasVelocity;
        private TransformRecorder _transformRecorder;
        public TransformRecorder TransformRecorder => _transformRecorder;
        
        public event Action DiceLanded;

        private Vector3 _cameraTransformPosition;
        private Quaternion _cameraTransformRotation;

        [SerializeField] public Vector3 PositionShift;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _transformRecorder = GetComponent<TransformRecorder>();
            if (Camera.main != null)
            {
                _cameraTransformPosition = Camera.main.transform.position + Vector3.down * 2 + PositionShift;
                _cameraTransformRotation = Camera.main.transform.rotation;
            }
        }

        public void SetSpeed(int speed)
        {
            _transformRecorder.SetNumberOfSkippedFrames(speed);
        }

        private void FixedUpdate()
        {
            if (_hasVelocity && _rb.velocity.magnitude <= 0.0001f)
            {
                DiceLanded?.Invoke();
                _hasVelocity = false;
            }

            if (!_rb.IsUnityNull() && !_hasVelocity && _rb.velocity.magnitude > 0)
            {
                _hasVelocity = true;
            }
        }

        public void Roll(float throwForce, float spinForce)
        {
            if(!_rb.IsUnityNull())
            {
                _rb.isKinematic = true;
                _rb.isKinematic = false;
                
                transform.position = _cameraTransformPosition;
                transform.rotation = _cameraTransformRotation;

                _rb.AddForce(Vector3.forward * throwForce, ForceMode.Impulse);
                _rb.AddTorque(UnityEngine.Random.insideUnitSphere * spinForce, ForceMode.Impulse);
            }
        }

        public int Result()
        {
            LayerMask.NameToLayer("Ground");
            if (Physics.Raycast( transform.position, transform.forward * -1, 1f)) return 1;
            if (Physics.Raycast( transform.position, transform.up * -1, 1f)) return 2;
            if (Physics.Raycast( transform.position, transform.right, 1f)) return 3;
            if (Physics.Raycast( transform.position, transform.right * -1, 1f)) return 4;
            if (Physics.Raycast( transform.position, transform.up, 1f)) return 5;
            if (Physics.Raycast( transform.position, transform.forward, 1f)) return 6;
            return 0;
        }
    }
}