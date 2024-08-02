using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

public class ReplayInstaller : MonoInstaller
{
    [SerializeField] private ReplayConfig _recallSpellConfig;

    public override void InstallBindings()
    {
        BindReplay();
    }

    private void BindReplay()
    {
        Container.Bind<ReplayConfig>().FromInstance(_recallSpellConfig).AsSingle();
    }
}