using UnityEngine;

public class TargetMarkerController : MonoBehaviour, IMoveable
{

    [SerializeField] private float speed = 5.0f;

    public void Move(Vector3 direction)
    {
        var zeroYDirection = new Vector3(direction.x, 0, direction.z);
        transform.position +=  speed * Time.deltaTime * zeroYDirection;
    }
}
