using UnityEngine;

public class IcebergLogic : MonoBehaviour
{
    [SerializeField] IcebergDataSO data;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if it is with the boat, call the boat "GetDamage" method
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boat"))
        {
            collision.gameObject.GetComponent<BoatLogic>().GetDamage(data.GetDamageAmount(), collision.GetContact(0).point);
        }
    }
}
