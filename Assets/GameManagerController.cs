using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    [SerializeField] private TargetMarkerController targetMarker;
    [SerializeField] private BallController ball;
    private IMoveable currentMoveable;
    
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
            ball.HandleBallHit(targetMarker.transform.position - ball.transform.position);
        }
    }
}
