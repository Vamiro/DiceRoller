using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Zenject;

public class ReplayInstaller : MonoInstaller
{
    [SerializeField] private ReplayConfig _replayConfig;

    public override void InstallBindings()
    {
        BindReplay();
    }

    private void BindReplay()
    {
        Container.Bind<ReplayConfig>().FromInstance(_replayConfig).AsSingle();
    }
}