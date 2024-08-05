using Dice;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class DiceInstaller : MonoInstaller
    {
        [SerializeField] private DiceConfig _diceConfig;

        public override void InstallBindings()
        {
            BindDice();
        }

        private void BindDice()
        {
            Container.Bind<DiceConfig>().FromInstance(_diceConfig).AsSingle();
        }
    }
}