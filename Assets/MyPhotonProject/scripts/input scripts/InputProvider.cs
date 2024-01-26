using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;


public class InputProvider : MonoBehaviour, IInputProvider
{
    private CustomInputAction _playerActionMap;
    public NetworkInputData newInputData;

    //NetworkInput networkInput;

    private bool _mouseButton0;

    public void Awake()
    {
        _playerActionMap = new CustomInputAction();
        newInputData = new NetworkInputData();
    }

    public void SetNetworkInput(NetworkInput input)
    {
        //networkInput = input;
        //_mouseButton0 = false;
        input.Set(newInputData);
        newInputData.buttons = 0;
        //newInputData = new NetworkInputData();

    }


    public void OnEnable()
    {
        _playerActionMap.Player.Enable();
        _playerActionMap.Player.Move.performed += ReadInput;
        _playerActionMap.Player.ButtonClick.performed += ReadMouseButtonClicks;


        /* var localNetworkRunner = FindObjectOfType<NetworkRunner>();
         if (localNetworkRunner != null)
         {
             // enabling the input map
             _playerActionMap.Player.Enable();
             _playerActionMap.Player.Move.performed += ReadInput;
             localNetworkRunner.AddCallbacks(this);
         }*/
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            Debug.Log("show when click is done ");
        _mouseButton0 = _mouseButton0 | Input.GetMouseButton(0);
    }

    public void ReadInput(InputAction.CallbackContext context)
    {
        newInputData.direction = context.ReadValue<Vector3>();
        Debug.Log("read input from input provider class " + newInputData.direction);
    }

    public void ReadMouseButtonClicks(InputAction.CallbackContext context)
    {
        _mouseButton0 = _mouseButton0 | Input.GetMouseButton(0);
        if (_mouseButton0)
            newInputData.buttons |= NetworkInputData.MOUSEBUTTON1;
        //Debug.Log("reading button clicks ");
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