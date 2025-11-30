using UnityEngine;

[CreateAssetMenu(fileName = "IcebergData", menuName = "Scriptable Objects/IcebergData")]
public class IcebergDataSO : ScriptableObject
{
    [SerializeField] ushort HitsCountBeforeDestruction;
    /// <summary>
    /// Damage (between [0 1]) caused by this iceberg when the boat collides it
    /// </summary>
    [SerializeField][Range(0f, 1f)] float DamageAmount;
    /// <summary>
    /// Number of (small) icebergs created when the iceberg is destroyed
    /// </summary>
    [SerializeField] ushort CreateIcebergsOnDestruction;

    public float GetDamageAmount()
    {
        return DamageAmount;
    }

    public ushort GetHitsCountBeforeDestruction()
    {
        return HitsCountBeforeDestruction;
    }

    public ushort GetIcebergsCreationCount()
    {
        return CreateIcebergsOnDestruction;
    }
}
