using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Dice
{
    [RequireComponent(typeof(Rigidbody))]
    public class DiceBehaviour : MonoBehaviour
    {
        private Rigidbody _rb;
        private Camera _camera;

        [SerializeField, Range(0f, 1000f)] private float _throwForce;
        [SerializeField, Range(0f, 15f)] private float _spinForce;
        [SerializeField, Range(0f, 5f)] private float _lerpSpeed;

        [SerializeField] private int desiredNum;
        [SerializeField] private Vector3 desiredSide;
        [SerializeField, Range(0f, 15f)] private int _speed;
        private bool _isGrounded;
        private bool _isLanded;

        void Start()
        {
            Time.timeScale = _speed;
            _camera = Camera.main;
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.red);
            if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                _isGrounded = true;
            }

            if (_isGrounded)
            {
                _rb.centerOfMass = desiredSide;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                UpdateDesiredSide();
                Time.timeScale = _speed;
                
                _isGrounded = false;
                _rb.centerOfMass = Vector3.zero;
                
                _rb.isKinematic = true;
                _rb.isKinematic = false;
                
                transform.position = _camera.transform.position + _camera.transform.forward * 5;
                transform.rotation = _camera.transform.rotation;
                
                _rb.AddForce(Vector3.forward * _throwForce, ForceMode.Impulse);
                _rb.AddTorque(UnityEngine.Random.insideUnitSphere * _spinForce, ForceMode.Impulse);
            }
        }
        
        private void UpdateDesiredSide()
        {
            switch (desiredNum)
            {

                case 1: desiredSide = Vector3.back; break;
                case 2: desiredSide = Vector3.down; break;
                case 3: desiredSide = Vector3.right; break;
                case 4: desiredSide = Vector3.left; break;
                case 5: desiredSide = Vector3.up; break;
                case 6: desiredSide = Vector3.forward; break;
                default: desiredSide = Vector3.back; break;
            }
        }
    }
}


/*
            if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                _isGrounded = true;
            }

            if (_isGrounded)
            {
                _rb.centerOfMass = Vector3.Lerp(_rb.centerOfMass, desiredSide, Time.deltaTime * _lerpSpeed);
            }
 
    */
