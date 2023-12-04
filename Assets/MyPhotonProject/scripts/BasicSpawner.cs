using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    //network object xy and id
    //networkcontroller to work with networt class update position - always call when i changed position
    //event in this class from network that someone moved it

    //facade to work INetworkRunnerCallbacks with networkcontroller


    private NetworkRunner _runner;

    private CustomInputAction _playerActionMap;

    //private DefaultInputActions _playerActionMap;
    NetworkInputData newInputData;

    //[SerializeField] InputProvider inputProvider;

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    public void Awake()
    {
        //_playerActionMap = new DefaultInputActions();
        _playerActionMap = new CustomInputAction();
        newInputData = new NetworkInputData();
    }


    public void OnEnable()
    {
        _playerActionMap.Player.Enable();
        _playerActionMap.Player.Move.performed += ReadInput;
    }
    public void ReadInput(InputAction.CallbackContext context)
    {
        newInputData.direction = context.ReadValue<Vector3>();
        //Debug.Log("oh, I'm confused");
        Debug.Log("read input from basic spawner " + newInputData.direction);
    }

    public void OnDisable()
    {
        _playerActionMap.Player.Disable();
    }

    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("show it on connection to server");
    }
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){ }
    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("I just wanna check if host migration happen automatically or I should code something here to make it happed");
    }
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {
        /*var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
            data.direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;

        input.Set(data);*/

        //Debug.Log("show me every time OnInput from Photon is called, I wanna know if I can set datat somewhere else");
        input.Set(newInputData); 
    }
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Let's see when player joined");
        if (runner.IsServer)
        {
            Debug.Log("seems like this is host");
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
            //spawn is similar to instaniate, but with ref on player, it will be needed for movement
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(player, networkPlayerObject);


            //here I've tried to make host colored //but it colored every object for host
            

            Renderer rend = networkPlayerObject.GetComponentInChildren<Renderer>();
            rend.material.color = Color.magenta;

        }
        else if(runner.IsClient)
        {
            Debug.Log("are you client?");

        }
        else
            Debug.Log("and who are you than????");
    }
    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("show me also when player left");
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("show it on load scene done");
        //from the test it will be shown only if player choose host, cos than scene would be loaded
    }
    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { }
    
    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }


    async void StartGame(GameMode mode)
    {
        //I was thinking I wanna have a separate game obj for runner cos I'll use it in input provider too, but runner shoud be null before start game// it can't exist on a scene
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        //inputProvider.enabled = true;


        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex, //this is only relevant for the host 
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            //as clients will be forced to use the scene specified by the host
        });

        }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
}
