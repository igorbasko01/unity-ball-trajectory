using UnityEngine;

public class BallController : MonoBehaviour, IMoveable
{
    [SerializeField] private float positioningSpeed = 5.0f;
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
        currentBallState?.HandleBallPositioning(direction, positioningSpeed);
    }

    public void ChangeState(BallState newState)
    {
        currentBallState?.Exit();
        currentBallState = newState;
        currentBallState?.Enter();
    }
}
