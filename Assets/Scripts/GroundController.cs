using UnityEngine;

public class GroundController : MonoBehaviour
{
    public Transform Car;
    
    void LateUpdate()
    {
        transform.position = new Vector3(Car.position.x, transform.position.y, Car.position.z);
    }
}
