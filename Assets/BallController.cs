using System;
using UnityEngine;

public class BallController : MonoBehaviour, IMoveable
{
    public delegate void MoveEvent(Vector3 direction);
    public event MoveEvent OnMove;
    private BallState currentBallState;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(new PositioningBallState(this));
    }

    // Update is called once per frame
    void Update()
    {
        currentBallState?.Update();
    }

    public void Move(Vector3 direction)
    {
        OnMove?.Invoke(direction);
    }

    public void ChangeState(BallState newState)
    {
        currentBallState?.Exit();
        currentBallState = newState;
        currentBallState?.Enter();
    }
}
