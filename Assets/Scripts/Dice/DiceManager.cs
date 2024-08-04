using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dice
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField] private List<DiceController> _diceControllers;

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
                    foreach (var diceController in _diceControllers)
                    {
                        diceController.PrecalculatedRoll();
                    }
                }
            }
        }
    }
}