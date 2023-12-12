using UnityEngine;
using Zenject;

public class MyInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _inputProvider;

    public override void InstallBindings()
    {
        //im not sure about this one
        Container.Bind<GameObject>().FromInstance(_playerPrefab);

        Container.Bind<IBasicSpawner>().To<BasicSpawner>().AsSingle();

        var inputProvider = Container.InstantiatePrefabForComponent<InputProvider>(_inputProvider);
        Container.Bind<IInputProvider>().FromInstance(inputProvider);
    }
}