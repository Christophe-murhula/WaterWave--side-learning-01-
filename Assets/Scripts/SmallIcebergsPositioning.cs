using Unity.VisualScripting;
using UnityEngine;

public class SmallIcebergsPositioning : MonoBehaviour
{
    EdgeCollider2D waterCollider;
    void Start()
    {
        waterCollider = FindFirstObjectByType<SpringsController>().GetComponent<EdgeCollider2D>();

    }
    void FixedUpdate()
    {
        RepositionOnY();
    }

    void RepositionOnY()
    {
        var myPos = transform.position;

        // set y position to nearest water collider's point.y
        var nearestPointOnWater = waterCollider.ClosestPoint(myPos);

        transform.position = new Vector3(myPos.x, nearestPointOnWater.y, myPos.z);
    }
}
