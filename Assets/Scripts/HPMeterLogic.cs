using UnityEngine;
using UnityEngine.U2D;

public class HPMeterLogic : MonoBehaviour
{
    // [SerializeField] GameObject waterMeter;
    [SerializeField] BoatLogic boatLogic;
    [SerializeField] SpriteShapeController waterLeveler;
    [SerializeField] float AdjustTopscreenOffset;

    Vector3 defaultTopleftPos, defaultToprightPos;
    const int topleftIndex = 1, toprightIndex = 2;
    const float waterLevelerHeight = 5f;
    float lastBoatHP = 0f;

    private void Start()
    {
        defaultTopleftPos = waterLeveler.spline.GetPosition(topleftIndex);
        defaultToprightPos = waterLeveler.spline.GetPosition(toprightIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // position on top right
        TopRightPositioning();
        // update waterMeter if the boat got damages
        UpdateWaterMeterSize();
    }

    void UpdateWaterMeterSize()
    {
        if (boatLogic.GetHP() != lastBoatHP)
        {
            lastBoatHP = boatLogic.GetHP();
            print(lastBoatHP);

            // update the topleft and topright y-coord of the spline
            var yValue = defaultTopleftPos.y + waterLevelerHeight * lastBoatHP;
            waterLeveler.spline.SetPosition(topleftIndex, new Vector3(defaultTopleftPos.x, yValue, defaultTopleftPos.z));
            waterLeveler.spline.SetPosition(toprightIndex, new Vector3(defaultToprightPos.x, yValue, defaultToprightPos.z));
        }
    }

    // NOTE: globalize mainCamera world boundaries getter
    void TopRightPositioning()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        // 1. Convert Viewport Edges to World Space
        // We use the nearClipPlane distance to project correctly into the world
        float zDistance = transform.position.z - mainCamera.transform.position.z;
        Vector3 screenLeftWorld = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 screenRightWorld = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDistance));


        // offset on X to stay X pixels from screenRight
        var offset = 1.2f;
        transform.position = new Vector3(screenRightWorld.x - offset, screenRightWorld.y + AdjustTopscreenOffset, transform.position.z);
    }
}
