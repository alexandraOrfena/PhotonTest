using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    [SerializeField] private Ball _prefabBall;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float spawnDelay = 0.5f;

    [Networked] private TickTimer delay { get;  set; }

    private NetworkCharacterControllerPrototype _cc;
    private Vector3 _forward;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterControllerPrototype>();
        _forward = transform.forward;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            MovePlayer(data);
            HandleBallSpawn(data);
        }
    }

    private void MovePlayer(NetworkInputData data)
    {
        if (data.direction.sqrMagnitude > 0)
        {
            data.direction.Normalize();
            _cc.Move(moveSpeed * data.direction * Runner.DeltaTime);
            _forward = data.direction;
        }
    }

    private void HandleBallSpawn(NetworkInputData data)
    {
        if (delay.ExpiredOrNotRunning(Runner) && (data.buttons & NetworkInputData.MOUSEBUTTON1) != 0)
        {
            delay = TickTimer.CreateFromSeconds(Runner, spawnDelay);
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        Runner.Spawn(_prefabBall, transform.position + _forward, Quaternion.LookRotation(_forward),
                     Object.InputAuthority, (runner, o) =>
                     {
                         o.GetComponent<Ball>().Init();
                     });
    }
}