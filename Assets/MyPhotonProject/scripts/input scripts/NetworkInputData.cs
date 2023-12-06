using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public const byte MOUSEBUTTON1 = 0x01;

    public byte buttons;

    //public bool buttons;

    public Vector3 direction;
}
