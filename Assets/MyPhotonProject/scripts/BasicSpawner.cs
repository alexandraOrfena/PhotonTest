using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Zenject;

public class BasicSpawner : IBasicSpawner
{
    [Inject]
    private GameObject _playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters { get; set; } = new Dictionary<PlayerRef, NetworkObject>();

    public void Spawn(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("seems like this is host");
        Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
        //spawn is similar to instaniate, but with ref on player, it will be needed for movement
        NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        // Keep track of the player avatars so we can remove it when they disconnect
        _spawnedCharacters.Add(player, networkPlayerObject);
        //here I've tried to make host colored
        //but it colored every object for host
        Renderer rend = networkPlayerObject.GetComponentInChildren<Renderer>();
        rend.material.color = Color.magenta;
    }

    public void Despawn(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
}