using UnityEngine;

public class TargetMarkerController : MonoBehaviour, IMoveable
{
    [SerializeField] private Transform boundingPlane;
    private float planeWidth;
    private float planeLength;
    [SerializeField] private float speed = 5.0f;

    public void Start() {
        planeWidth = boundingPlane.localScale.x * 10;
        planeLength = boundingPlane.localScale.z * 10;
    }
    
    public void Move(Vector3 direction)
    {
        MoveMarker(direction);
        BindToBounds();
    }

    private void MoveMarker(Vector3 direction) {
        var zeroYDirection = new Vector3(direction.x, 0, direction.z);
        transform.position +=  speed * Time.deltaTime * zeroYDirection;
    }

    private void BindToBounds() {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, boundingPlane.position.x - planeWidth / 2, boundingPlane.position.x + planeWidth / 2);
        position.z = Mathf.Clamp(position.z, boundingPlane.position.z - planeLength / 2, boundingPlane.position.z + planeLength / 2);

        transform.position = position;
    }
}
