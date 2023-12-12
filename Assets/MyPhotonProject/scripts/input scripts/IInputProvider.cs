using Fusion;
using UnityEngine.InputSystem;

public interface IInputProvider
{
    void SetNetworkInput(NetworkInput input);
    void ReadInput(InputAction.CallbackContext context);
    void ReadMouseButtonClicks(InputAction.CallbackContext context);
}
