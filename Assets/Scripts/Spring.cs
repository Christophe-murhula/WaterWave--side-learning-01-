using System;
using UnityEngine;
using UnityEngine.U2D;

public class Spring : MonoBehaviour
{
    private Spline spline;
    private Vector3 WAVE_DEFAULT_POINT = new(0, 0, -1);
    private int waves_points_count;
    int wave_index;
    private float velocity = 0f, force = 0f, height = 0f, target_height = 0f;

    Rigidbody2D myBody;
    const float DEFAULT_VELOCITY = 0.05f;
    float velocity_x = 0;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        velocity_x = DEFAULT_VELOCITY * Mathf.Sign(UnityEngine.Random.Range(-1, 2));
    }

    public void Init(Spline s, int waves_count, Vector3 point, int index)
    {
        spline = s;
        waves_points_count = waves_count;
        wave_index = index;

        WAVE_DEFAULT_POINT = new Vector3(point.x, point.y, point.z);
        height = point.y;
        target_height = height;
    }

    void FixedUpdate()
    {
        float velocity_modifier = 1.2f * Mathf.Abs(WAVE_DEFAULT_POINT.y - transform.localPosition.y);
        myBody.linearVelocityX = velocity_x + Mathf.Sign(velocity_x) * velocity_modifier;

        if (myBody.linearVelocityY != 0f)
        {
            myBody.linearVelocityY = Mathf.Lerp(myBody.linearVelocityY, 0f, .05f);
        }
    }

    public void HorizontalMovementUpdate(float gap)
    {
        if (WAVE_DEFAULT_POINT.z == -1) { return; }

        // change velocity direction if we come close to neigbors
        var pos_x = transform.localPosition.x;
        // var tolerance = .1f;

        if (velocity_x < 0 && (WAVE_DEFAULT_POINT.x - pos_x) >= gap)
        {
            velocity_x = DEFAULT_VELOCITY;
        }
        else if (velocity_x > 0 && (pos_x - WAVE_DEFAULT_POINT.x) >= gap)
        {
            velocity_x = -DEFAULT_VELOCITY;
        }
    }

    public void WaveSpringUpdate(float spring_stiffness, float damping)
    {
        Vector3 localPos = transform.localPosition;

        height = localPos.y;
        // max extension
        var y = height - target_height;
        var loss = -damping * velocity;

        force = -spring_stiffness * y + loss;
        velocity += force;

        transform.localPosition = new Vector3(localPos.x, height + velocity, localPos.z);

        // update attached spline point
        try
        {
            spline.SetPosition(wave_index, transform.localPosition);
        }
        catch (ArgumentException)
        {
            return;
        }
    }

    public float Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }

    public float Height
    {
        get
        {
            return height;
        }
    }
}
