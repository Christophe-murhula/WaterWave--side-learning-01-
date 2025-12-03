using UnityEngine;

public class DropDestruction : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            Destroy(gameObject);
        }
    }
}
