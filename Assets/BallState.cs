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

    public abstract void HandleBallPositioning(Vector3 direction, float positioningSpeed);
}

public class PositioningBallState : BallState
{
    private Rigidbody rb;

    public PositioningBallState(BallController ballController) : base(ballController)
    {
        rb = ballController.GetComponent<Rigidbody>();
    }

    public override void Enter()
    {
        Debug.Log("Entering Positioning State");
        rb.isKinematic = true;
    }

    public override void Update()
    {
        // Do nothing
    }

    public override void Exit()
    {
        Debug.Log("Exiting Positioning State");
    }

    public override void HandleBallPositioning(Vector3 direction, float positioningSpeed)
    {
        ballController.transform.position += positioningSpeed * Time.deltaTime * direction;
    }
}
