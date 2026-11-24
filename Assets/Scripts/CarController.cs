using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction qPressed;

    private float friction = 0.5f;
    private float tractionMax = 20f;
    private float reverseMax = 15f;
    private float powerMax = 600f;
    private float brake = 40f;

    private float windCoefficient = 0.001f;
    private float turnMax = 30f;
    private float turnSpeed = 5f;
    private float returnSpeed = 1.5f;

    private const float wheelbase = 2.62f;

    private float speed = 0f;
    private float turnAngle = 0f;
    private bool reverse = false;

    private float GetAcceleration(float move)
    {
        float resistance = friction + windCoefficient * speed * speed;
        if (move > 0)
        {
            float forceMax = reverse ? reverseMax : tractionMax;
            if (speed * forceMax < powerMax)
            {
                return forceMax - resistance;
            }
            else
            {
                return powerMax / speed - resistance;
            }
        } else if (move < 0)
        {
            return -brake - resistance;
        } else
        {
            return -resistance;
        }
    }

    private float GetDeltaDistance(float move)
    {
        float a = GetAcceleration(move);
        float t = Time.deltaTime;
        float d = speed * t + 0.5f * a * t * t;
        d = d < 0 ? 0 : d;
        return reverse ? -d : d;
    }

    private float GetSpeed(float move)
    {
        float a = GetAcceleration(move);
        float t = Time.deltaTime;
        float v = speed + a * t;

        if (v < 0)
        {
            v = 0;
        }

        return v;
    }

    private void GearShift()
    {
        if (speed == 0)
        {
            reverse = !reverse;
        }
    }

    private float GetTurnAngle(float turn)
    {
        if (turn == 0)
        {
            float tmp2;
            if (turnAngle > 0)
            {
                tmp2 = turnAngle - returnSpeed * Time.deltaTime;
                if (tmp2 < 0)
                {
                    tmp2 = 0;
                }
            }
            else
            {
                tmp2 = turnAngle + returnSpeed * Time.deltaTime;
                if (tmp2 > 0)
                {
                    tmp2 = 0;
                }
            }
            return tmp2;
        }
        float tmp = turnAngle + turn * turnSpeed * Time.deltaTime;
        if (tmp > turnMax)
        {
            tmp = turnMax;
        }
        else if (tmp < -turnMax)
        {
            tmp = -turnMax;
        }
        return tmp;
    }

    private float GetDeltaAngle(float distance)
    {
        return Mathf.Rad2Deg * distance * Mathf.Tan(Mathf.Deg2Rad * turnAngle) / wheelbase;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public bool IsReverse()
    {
        return reverse;
    }

    void OnEnable()
    {
        moveAction.Enable();
        qPressed.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            if (qPressed.triggered)
            {
                GearShift();
            }

            Vector2 move = moveAction.ReadValue<Vector2>();

            //if (move.x != 0 && move.y !=0)
            //{
            //    Debug.Log(move);
            //}

            float d = GetDeltaDistance(move.y);
            speed = GetSpeed(move.y);
            transform.Translate(0, 0, d);

            float a = GetDeltaAngle(d);
            transform.Rotate(0, a, 0);

            turnAngle = GetTurnAngle(move.x);
        }
    }
}
