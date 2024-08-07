using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Dice
{
    public class DiceManager : MonoBehaviour
    {
        private List<DiceController> _diceControllers;
        private int _desiredNum;

        [Inject]
        public void Construct(DiceConfig config, List<DiceController> diceControllers)
        {
            _desiredNum = config.DesiredNum;
            _diceControllers = diceControllers;
        }
        
        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _diceControllers.All(controller => controller.CanRoll))
            {
                var num = SplitNum();
                
                foreach (var diceController in _diceControllers)
                {
                    diceController.PrecalculatedRoll(num);
                    num = _desiredNum - num;
                }
            }
        }

        private int SplitNum()
        {
            int x = 6;
            int y = 2;
            int num = _desiredNum > x ? 
                _desiredNum < x * y ? UnityEngine.Random.Range(_desiredNum % x, x + 1) : x
                : UnityEngine.Random.Range(1, _desiredNum);
            return num;
        }
    }
}
