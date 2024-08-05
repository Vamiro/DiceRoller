using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Dice
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField] private List<DiceController> _diceControllers;
        private int _desiredNum;
        public int DesiredNum { get; set; }

        [Inject]
        public void Construct(DiceConfig config)
        {
            _desiredNum = config.DesiredNum;
        }
        
        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bool res = true;
                foreach (var diceController in _diceControllers)
                {
                    res = res == diceController.CanRoll;
                }

                if (res)
                {
                    int num = _desiredNum > 6 ? 
                        _desiredNum < 12 ? UnityEngine.Random.Range(_desiredNum % 6, 6 + 1) : 6
                        : UnityEngine.Random.Range(1, _desiredNum);
                    
                    foreach (var diceController in _diceControllers)
                    {
                        diceController.PrecalculatedRoll(num);
                        num = _desiredNum - num;
                    }
                }
            }
        }
    }
}