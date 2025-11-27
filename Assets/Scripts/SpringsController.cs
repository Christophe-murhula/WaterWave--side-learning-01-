using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpringsController : MonoBehaviour
{
    [SerializeField] GameObject spring_object;
    [SerializeField] SpriteShapeController waterSpriteShape;
    [SerializeField] Transform springs_points_transform;


    [SerializeField] float spring_stiffness;
    [SerializeField] float damping;
    [SerializeField] float spread;
    [SerializeField] int waves_count;
    float spacing_per_wave;

    List<Spring> springs = new();
    const int corners_count = 2;

    void Start()
    {
        SetWaves();
    }

    void SetWaves()
    {
        Spline waterSpline = waterSpriteShape.spline;
        // NOTE: spline points are clockwise
        Vector3 top_left_corner = waterSpline.GetPosition(1), top_right_corner = waterSpline.GetPosition(2);
        float WATER_WIDTH = top_right_corner.x - top_left_corner.x;

        spacing_per_wave = WATER_WIDTH / (waves_count + 1);

        List<Vector3> points = new();
        // create spline points & attach spring object
        for (int i = waves_count; i > 0; i--)
        {
            int index = corners_count;
            // create spline point
            float x_pos = top_left_corner.x + (spacing_per_wave * i);
            Vector3 wave_point = new(x_pos, top_left_corner.y, top_left_corner.z);
            points.Add(wave_point);
            waterSpline.InsertPointAt(index, wave_point);
            waterSpline.SetHeight(index, .1f);
            waterSpline.SetCorner(index, false);
            waterSpline.SetTangentMode(index, ShapeTangentMode.Continuous);
            waterSpline.SetLeftTangent(index, new Vector3(-.5f, 0f, 0f));
            waterSpline.SetRightTangent(index, new Vector3(.5f, 0f, 0f));

            //attach spring to it
            var spring_obj = Instantiate(spring_object, springs_points_transform, false);
            spring_obj.transform.localPosition = wave_point;
            var spring = spring_obj.GetComponent<Spring>();
            spring.Init(waterSpline, wave_point, index + i - 1);
            springs.Add(spring);
        }
    }

    void FixedUpdate()
    {
        foreach (Spring spring in springs)
        {
            spring.WaveSpringUpdate(spring_stiffness, damping);
            spring.HorizontalMovementUpdate(spacing_per_wave / 4f);
        }
        UpdateSprings();
    }

    private void UpdateSprings()
    {
        int count = springs.Count;
        float[] left_deltas = new float[count], right_deltas = new float[count];

        for (int i = 0; i < count; i++)
        {
            if (i > 0)
            {
                left_deltas[i] = spread * (springs[i].Height - springs[i - 1].Height);
                springs[i - 1].Velocity += left_deltas[i] * Random.Range(.8f, 1.2f);
            }
            if (i < count - 1)
            {
                right_deltas[i] = spread * (springs[i].Height - springs[i + 1].Height);
                springs[i + 1].Velocity += right_deltas[i];
            }
        }
    }
}
