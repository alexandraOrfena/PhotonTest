using Fusion;

public interface IBasicSpawner
{
    void Spawn(NetworkRunner runner, PlayerRef player);
    void Despawn(NetworkRunner runner, PlayerRef player);
}
