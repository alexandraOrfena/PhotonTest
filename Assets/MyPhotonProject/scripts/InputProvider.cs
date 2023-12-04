using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.InputSystem;
using System;

public class InputProvider : MonoBehaviour
{
    DefaultInputActions _playerActionMap;

    public void Awake()
    {
        _playerActionMap = new DefaultInputActions();
    }


    public void OnEnable()
    {
        _playerActionMap.Player.Enable();
        _playerActionMap.Player.Move.performed += ReadInput;
       /* var localNetworkRunner = FindObjectOfType<NetworkRunner>();
        if (localNetworkRunner != null)
        {
            // enabling the input map
            _playerActionMap.Player.Enable();
            _playerActionMap.Player.Move.performed += ReadInput;
            localNetworkRunner.AddCallbacks(this);
        }*/
    }

    public void ReadInput(InputAction.CallbackContext context)
    {
        var tmp = context.ReadValue<Vector2>();
        Debug.Log("oh, I'm confused");
        Debug.Log("show me this "+tmp);
    }

    /*public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log(" i'm reading input from InputProvider class");
        // Same as in the snippet for SimulationBehaviour and NetworkBehaviour.
    }*/

    public void OnDisable()
    {
        _playerActionMap.Player.Disable();
        /*if (localNetworkRunner != null)
        {
            // disabling the input map
            _playerActionMap.Player.Disable();

            localNetworkRunner.RemoveCallbacks(this);
        }*/
    }

    /*public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }*/

  
}
