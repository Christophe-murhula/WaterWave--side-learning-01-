using UnityEngine;
using UnityEngine.U2D;

public class HPMeterLogic : MonoBehaviour
{
    // [SerializeField] GameObject waterMeter;
    [SerializeField] BoatLogic boatLogic;
    [SerializeField] SpriteShapeController waterLeveler;
    [SerializeField] float AdjustTopscreenOffset;

    WaterDropsLogic waterDropsLogic;

    Vector3 defaultTopleftPos, defaultToprightPos;
    const int topleftIndex = 1, toprightIndex = 2;
    const float waterLevelerHeight = 5f;
    /// <summary>
    /// The value of the y-coordinate of the two top-points of the waterLeveler spline
    /// </summary>
    float yValue;

    float lastBoatHP = 0f;


    private void Start()
    {
        waterDropsLogic = GetComponent<WaterDropsLogic>();

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
        var currentBoatHP = boatLogic.GetHP();
        if (currentBoatHP != lastBoatHP)
        {
            // set the number of drops to the quantity of damage taken times 10
            float damageQuantity = lastBoatHP - currentBoatHP;
            if (damageQuantity > 0f)
            {
                int q = Mathf.RoundToInt(damageQuantity * 10);

                waterDropsLogic.RechargeDrops((ushort)(q * 6));
            }


            lastBoatHP = currentBoatHP;

            // update the topleft and topright y-coord of the spline
            yValue = defaultTopleftPos.y + waterLevelerHeight * lastBoatHP;
        }

        // lerp the y-value when it changes
        var topleftY = waterLeveler.spline.GetPosition(topleftIndex).y;
        if (topleftY != yValue)
        {
            var yValueLerp = Mathf.Lerp(topleftY, yValue, 0.05f);
            waterLeveler.spline.SetPosition(topleftIndex, new Vector3(defaultTopleftPos.x, yValueLerp, defaultTopleftPos.z));
            waterLeveler.spline.SetPosition(toprightIndex, new Vector3(defaultToprightPos.x, yValueLerp, defaultToprightPos.z));
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
