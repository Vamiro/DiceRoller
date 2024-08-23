using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dice
{
    public class DiceComponent : MonoBehaviour
    {
        private const int CubeLayerIndex = 7;
        public static readonly List<Vector3> _diceSides = new()
        {
            Vector3.forward,
            Vector3.up,
            Vector3.left,
            Vector3.right,
            Vector3.down,
            Vector3.back
        };

        private Rigidbody _rb;
        private bool _hasVelocity;
        private Transform _startTransform;

        public event Action OnDiceLanded;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Initialize(Transform startTransform)
        {
            _startTransform = startTransform;
        }

        private void FixedUpdate()
        {
            if (_hasVelocity && _rb.velocity.magnitude <= 0.0001f)
            {
                OnDiceLanded?.Invoke();
                _hasVelocity = false;
            }

            if (!_rb.IsUnityNull() && !_hasVelocity && _rb.velocity.magnitude > 0)
            {
                _hasVelocity = true;
            }
        }

        public void Roll(float throwForce, float spinForce)
        {
            if (!_rb.IsUnityNull())
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
            const int layerMask = 1 << CubeLayerIndex;
            
            for (int i = 0; i < 6; i++)
            {
                if (Physics.Raycast(transform.position, transform.TransformVector(_diceSides[i]), 
                        transform.localScale.x, layerMask))
                {
                    return 6 - i;
                }
            }
            return -1;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int j = 0; j < 6; j++)
            {
                Gizmos.DrawLine(transform.position,transform.position + transform.TransformVector(_diceSides[j]));
            }
        }
    }
}
