using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform car;
    private Vector3 focusOffset = new Vector3(0, 1, 0);
    private float distance = 10f;

    private float sensitivity = 0.1f;

    private float pitch = 10;
    private float yaw = 0;
    

    private Vector3 GetOffset()
    {
        Vector3 res = car.forward;
        res = Quaternion.AngleAxis(pitch + 180f, car.right) * res;
        res = Quaternion.Euler(0, yaw, 0) * res;

        float actualDistance = distance;
        if (distance * res.y + car.position.y + focusOffset.y < 0.0001 && res.y < 0)
        {
            actualDistance = (0.0001f - car.position.y - focusOffset.y) / res.y;
        }

        return actualDistance * res;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pitch += -Mouse.current.delta.y.ReadValue() * sensitivity * Time.timeScale;
        if (pitch < -89)
        {
            pitch = -89f;

        }
        else if (pitch > 89)
        {
            pitch = 89f;

        }

        yaw += Mouse.current.delta.x.ReadValue() * sensitivity * Time.timeScale;
        while (yaw < -180)
        {
            yaw += 360;
        }
        while (yaw > 180)
        {
            yaw -= 360;
        }
    }

    void LateUpdate()
    {
        transform.position = car.position + focusOffset + GetOffset();
        transform.LookAt(car.position + focusOffset);
    }
}
