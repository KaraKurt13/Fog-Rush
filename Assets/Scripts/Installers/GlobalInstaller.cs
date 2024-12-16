using Assets.Scripts.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<DatabaseManager>().AsSingle();
        Container.Bind<LevelManager>().AsSingle();
    }
}
