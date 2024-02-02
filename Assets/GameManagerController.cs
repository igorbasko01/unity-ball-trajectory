using System;
using TMPro;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    [SerializeField] private TargetMarkerController targetMarker;
    [SerializeField] private BallController ball;
    [SerializeField] private TextMeshProUGUI ballFlightTimeText;
    [SerializeField] private Transform netIntersectionMarker;
    [SerializeField] private TextMeshProUGUI debugText;
    private IMoveable currentMoveable;
    private float ballFlightSeconds = 0.5f;
    public float BallFlightSeconds => ballFlightSeconds;
    private float gravity = -9.81f;
    
    // Start is called before the first frame update
    void Start()
    {
        currentMoveable = targetMarker;
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveY = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            moveY = 1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            moveY = -1;
        }
        Vector3 movement = new(moveHorizontal, moveY, moveVertical);
        currentMoveable.Move(movement);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentMoveable == (IMoveable) targetMarker)
            {
                currentMoveable = ball;
            }
            else
            {
                currentMoveable = targetMarker;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ball.HandleBallHit(CalculateForce());
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            ballFlightSeconds -= 0.1f;
            ballFlightSeconds = Mathf.Round(ballFlightSeconds * 10) / 10;
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            ballFlightSeconds += 0.1f;
            ballFlightSeconds = Mathf.Round(ballFlightSeconds * 10) / 10;
        }
        var willClearNet = WillClearNet(CalculateDistanceToNet(), 1.1f);
        ballFlightTimeText.text = $"Ball Flight Time: {ballFlightSeconds}s, Clear Net: {willClearNet}";
        PlaceNetIntersectionMarker();
    }

    private Vector3 CalculateForce() {
        var targetPosition = targetMarker.transform.position;
        var ballPosition = ball.transform.position;
        var distanceX = targetPosition.x - ballPosition.x;
        var distanceZ = targetPosition.z - ballPosition.z;
        var distanceY = targetPosition.y - ballPosition.y;
        var velocityX = distanceX / ballFlightSeconds;
        var velocityZ = distanceZ / ballFlightSeconds;
        var velocityY = (distanceY - 0.5f * gravity * ballFlightSeconds * ballFlightSeconds) / ballFlightSeconds;
        return new Vector3(velocityX, velocityY, velocityZ);
    }

    private Vector3 CalculateNetIntersectionMarker() {
        Vector3 ballPosition = ball.transform.position;
        Vector3 targetPosition = targetMarker.transform.position;
        Vector2 p1 = new(ballPosition.x, ballPosition.z);
        Vector2 p2 = new(targetPosition.x, targetPosition.z);
        Vector2 p3 = new(-1000, 0);
        Vector2 p4 = new(1000, 0);
        Vector2 netIntersectionPosition = Vector2.zero;
        float denominator = (p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y);
        if (denominator == 0)
        {
            return netIntersectionPosition;
        }
        float ua = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) / denominator;
        netIntersectionPosition.x = p1.x + ua * (p2.x - p1.x);
        netIntersectionPosition.y = p1.y + ua * (p2.y - p1.y);
        return new(netIntersectionPosition.x, 0, netIntersectionPosition.y);
    }

    private void PlaceNetIntersectionMarker() {
        netIntersectionMarker.position = CalculateNetIntersectionMarker();
    }

    private bool WillClearNet(float distanceToNet, float netHeight) {
        Vector3 initialVelocity = CalculateForce();

        float timeToNet = Math.Abs(distanceToNet) / Math.Abs(initialVelocity.z);

        float ballHeight = ball.transform.position.y;

        float heightAtNet = ballHeight + initialVelocity.y * timeToNet + 0.5f * gravity * timeToNet * timeToNet;

        debugText.text = $"Distance to net: {distanceToNet:F2}, height at net: {heightAtNet:F2}, initial height: {ballHeight:F2}, time to net: {timeToNet:F2}";

        return heightAtNet > netHeight;
    }

    private float CalculateDistanceToNet() {
        Vector3 ballPosition = ball.transform.position;
        Vector3 ballWithoutHeight = new(ballPosition.x, 0, ballPosition.z);
        Vector3 intersectionPosition = CalculateNetIntersectionMarker();
        return Vector3.Distance(ballWithoutHeight, intersectionPosition);
    }
}
