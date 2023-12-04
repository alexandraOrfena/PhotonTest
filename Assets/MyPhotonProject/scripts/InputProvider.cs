using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.InputSystem;
using System;

public class InputProvider : MonoBehaviour
{
    private CustomInputAction _playerActionMap;
    NetworkInputData newInputData;

    NetworkInput networkInput;

    public void Awake()
    {
        _playerActionMap = new CustomInputAction();
        newInputData = new NetworkInputData();
    }

    public void SetNetworkInput(NetworkInput input)
    {
        networkInput = input;
        input.Set(newInputData);
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
        newInputData.direction = context.ReadValue<Vector3>();
        Debug.Log("read input from input provider class " + newInputData.direction);
    }

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

}