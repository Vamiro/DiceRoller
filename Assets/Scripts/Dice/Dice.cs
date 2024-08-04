using System;
using Unity.VisualScripting;
using UnityEngine;

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

        private Transform _startTransform;
        
        [SerializeField] public Vector3 PositionShift;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _transformRecorder = GetComponent<TransformRecorder>();
        }

        public void Initialize(Transform startTransform, int speed)
        {
            _startTransform = startTransform;
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
                
                transform.position = _startTransform.position;
                transform.rotation = _startTransform.rotation;

                _rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
                _rb.AddTorque(UnityEngine.Random.insideUnitSphere * spinForce, ForceMode.Impulse);
            }
        }

        public int Result()
        {
            int cubeLayerIndex = LayerMask.NameToLayer("Ground");

            if (cubeLayerIndex == -1)
            {
                Debug.LogError("Ground layer did not found");
            }
            else
            {
                int layerMask = 1 << cubeLayerIndex;
                if (Physics.Raycast(transform.position, transform.forward * -1, 1f, layerMask)) return 1;
                if (Physics.Raycast(transform.position, transform.up * -1, 1f, layerMask)) return 2;
                if (Physics.Raycast(transform.position, transform.right, 1f, layerMask)) return 3;
                if (Physics.Raycast(transform.position, transform.right * -1, 1f, layerMask)) return 4;
                if (Physics.Raycast(transform.position, transform.up, 1f, layerMask)) return 5;
                if (Physics.Raycast(transform.position, transform.forward, 1f, layerMask)) return 6;
            }
            return 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * -1);
            Gizmos.DrawLine(transform.position, transform.position + transform.up * -1);
            Gizmos.DrawLine(transform.position, transform.position + transform.right);
            Gizmos.DrawLine(transform.position, transform.position + transform.right * -1);
            Gizmos.DrawLine(transform.position, transform.position + transform.up);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }
    }
}