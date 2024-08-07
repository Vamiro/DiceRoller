using System.Collections.Generic;
using Dice;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class DiceInstaller : MonoInstaller
    {
        [SerializeField] private ReplayConfig _replayConfig;
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
            Container.Bind<ReplayConfig>().FromInstance(_replayConfig).AsSingle();
        }
    }
}