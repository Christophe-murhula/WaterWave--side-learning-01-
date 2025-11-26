using UnityEngine;

public class PlayerScreenBoundary : MonoBehaviour
{
    CapsuleCollider2D myBodyCollider;
    private float playerHalfWidth;

    void Start()
    {
        // Get the half-width of the player's Collider2D
        myBodyCollider = GetComponent<CapsuleCollider2D>();

        playerHalfWidth = myBodyCollider.bounds.extents.x;
    }

    void LateUpdate()
    {
        ClampPlayerPosition();
    }

    void ClampPlayerPosition()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        // 1. Convert Viewport Edges to World Space
        // We use the nearClipPlane distance to project correctly into the world
        float zDistance = transform.position.z - mainCamera.transform.position.z;
        Vector3 screenLeftWorld = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDistance));
        Vector3 screenRightWorld = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDistance));

        // 2. Calculate Final Clamping Limits (adjusting for player's half-width)
        float minX = screenLeftWorld.x + playerHalfWidth;
        float maxX = screenRightWorld.x - playerHalfWidth;

        // 3. Apply Clamping
        // Get the player's current position
        Vector3 playerPos = transform.position;

        // Clamp the player's X position between minX and maxX
        playerPos.x = Mathf.Clamp(playerPos.x, minX, maxX);

        // Apply the clamped position back to the player
        transform.position = playerPos;
    }
}
