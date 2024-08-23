using System.Collections.Generic;
using Dice;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers
{
    public class DiceInstaller : MonoInstaller
    {
        [SerializeField] private RecorderConfig recorderConfig;
        [SerializeField] private DiceConfig _diceConfig;
        [SerializeField] private List<DiceController> _diceControllers;
        
        public override void InstallBindings()
        {
            BindDice();
            BindReplay();
        }

        private void BindDice()
        {
            Container.Bind<DiceConfig>().FromInstance(_diceConfig).AsSingle();
            Container.Bind<List<DiceController>>().FromInstance(_diceControllers).AsSingle();
        }
        
        private void BindReplay()
        {
            Container.Bind<RecorderConfig>().FromInstance(recorderConfig).AsSingle();
        }
    }
}
