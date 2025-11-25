using UnityEngine;

public class ConstantWaterPosition : MonoBehaviour
{
    // get the boat's camera transform
    [SerializeField] Transform boatCameraTransform;
    [SerializeField] float xOffset;

    void Update()
    {
        var pos = transform.position;
        transform.position = new Vector3(boatCameraTransform.position.x + xOffset, pos.y, pos.z);
    }
}