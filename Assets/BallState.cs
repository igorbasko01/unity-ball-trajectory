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
