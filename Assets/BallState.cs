using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BallState : IState
{
    protected BallController ballController;

    public BallState(BallController ballController)
    {
        this.ballController = ballController;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

public class PositioningBallState : BallState
{
    private readonly float positioningSpeed = 5.0f;
    private readonly Rigidbody rb;

    public PositioningBallState(BallController ballController) : base(ballController)
    {
        rb = ballController.GetComponent<Rigidbody>();
    }

    public override void Enter()
    {
        Debug.Log("Entering Positioning State");
        rb.isKinematic = true;
        ballController.OnMove += HandleBallPositioning;
    }

    public override void Update()
    {
        // Do nothing
    }

    public override void Exit()
    {
        Debug.Log("Exiting Positioning State");
        rb.isKinematic = false;
        ballController.OnMove -= HandleBallPositioning;
    }

    private void HandleBallPositioning(Vector3 direction)
    {
        ballController.transform.position += positioningSpeed * Time.deltaTime * direction;
    }
}

public class MovingBallState : BallState
{
    public event Action OnTimeElapsed;
    private Vector3 initialPosition;
    private float timer = 0;
    private readonly float timeToWait = 3.0f;
    private readonly Vector3 initialHitForce;
    private readonly Rigidbody rb;
    public MovingBallState(BallController ballController, Vector3 hitForce) : base(ballController)
    {
        initialHitForce = hitForce;
        rb = ballController.GetComponent<Rigidbody>();
    }
    public override void Enter()
    {
        Debug.Log("Entering Moving State");
        timer = 0;
        initialPosition = ballController.transform.position;
        OnTimeElapsed += ballController.HandleMovingStateCompleted;
        rb.isKinematic = false;
        rb.AddForce(initialHitForce, ForceMode.Impulse);
    }

    public override void Exit()
    {
        OnTimeElapsed -= ballController.HandleMovingStateCompleted;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ballController.transform.position = initialPosition;
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToWait)
        {
            OnTimeElapsed?.Invoke();
            timer = 0;
        }
    }
}
